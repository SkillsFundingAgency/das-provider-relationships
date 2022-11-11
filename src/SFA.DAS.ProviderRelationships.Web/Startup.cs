using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using NLog;
using Owin;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.OidcMiddleware;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web;
using SFA.DAS.ProviderRelationships.Web.App_Start;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Authentication.GovUk.Services;

[assembly: OwinStartup(typeof(Startup))]

namespace SFA.DAS.ProviderRelationships.Web
{
    public class Startup
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public void Configuration(IAppBuilder app)
        {
            var container = StructuremapMvc.StructureMapDependencyScope.Container;
            var oidcConfiguration = container.GetInstance<IOidcConfiguration>();
            var govUkOidcConfiguration = container.GetInstance<GovUkOidcConfiguration>();
            var authenticationUrls = container.GetInstance<IAuthenticationUrls>();
            var postAuthenticationHandler = container.GetInstance<IPostAuthenticationHandler>();
            var providerRelationshipsConfiguration = container.GetInstance<ProviderRelationshipsConfiguration>();
            
            Logger.Info("Starting Provider Relationships web application");
            Logger.Info("Initializing Authentication");

            // Use SystemWebCookieManager to prevent conflict between
            // Owin Cookies modifying collection via Set-Cookie
            // And System.Web modifying Response.Cookies Collection
            // https://web.archive.org/web/20170912171644/https:/katanaproject.codeplex.com/workitem/197

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                CookieName = "provider-relationships",
                AuthenticationType = "Cookies",
                ExpireTimeSpan = new TimeSpan(0, 10, 0),
                SlidingExpiration = true,
                CookieManager = new SystemWebCookieManager()
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                CookieName = "provider-relationships-temp",
                AuthenticationType = "TempState",
                AuthenticationMode = AuthenticationMode.Passive,
                CookieManager = new SystemWebCookieManager()
            });

            if (providerRelationshipsConfiguration is { UseGovUkSignIn: true })//todo: use SFA.DAS.FeatureToggle once upgraded to .net6
            {
                var handler = new JwtSecurityTokenService(govUkOidcConfiguration);
                
                app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions("code") {
                    ClientId = govUkOidcConfiguration.ClientId,
                    Scope = "openid email phone",
                    Authority = govUkOidcConfiguration.BaseUrl,
                    MetadataAddress = $"{govUkOidcConfiguration.BaseUrl}/.well-known/openid-configuration",
                    ResponseType = OpenIdConnectResponseType.Code,
                    ResponseMode = "",
                    SaveTokens = true,
                    RedeemCode = true,
                    RedirectUri =  providerRelationshipsConfiguration.ApplicationBaseUrl + "/sign-in",
                    UsePkce = false,
                    CookieManager = new ChunkingCookieManager(),
                    SignInAsAuthenticationType = "Cookies",
                    SecurityTokenValidator = handler,
                    Notifications = new OpenIdConnectAuthenticationNotifications {
                        AuthorizationCodeReceived = notification =>
                        {
                            var code = notification.Code;
                            var redirectUri = notification.RedirectUri;
                            var oidcService = new OidcService(
                                new HttpClient(), 
                                new AzureIdentityService(), 
                                handler,
                                new GovUkOidcConfiguration {
                                    BaseUrl = notification.Options.Authority,
                                    ClientId = notification.Options.ClientId,
                                    KeyVaultIdentifier = govUkOidcConfiguration.KeyVaultIdentifier
                                });

                            var result = oidcService.GetToken(code, redirectUri);
                            var claims = new List<Claim> {
                                new Claim("id_token", result.IdToken),
                                new Claim("access_token", result.AccessToken),
                                new Claim("expires_at", DateTime.UtcNow.AddMinutes(10).ToString(CultureInfo.CurrentCulture))
                            };
                            var claimsIdentity = new ClaimsIdentity(claims, notification.Options.SignInAsAuthenticationType);
                            notification.HandleCodeRedemption(result.AccessToken, result.IdToken);
                            var properties =
                                notification.Options.StateDataFormat.Unprotect(notification.ProtocolMessage.State.Split('=')[1]);
                            notification.AuthenticationTicket = new AuthenticationTicket(claimsIdentity, properties);
                            
                            return Task.CompletedTask;
                        },
                        SecurityTokenValidated = notification =>
                        {
                            var oidcService = new OidcService(
                                new HttpClient(), 
                                new AzureIdentityService(), 
                                handler,
                                new GovUkOidcConfiguration {
                                    BaseUrl = notification.Options.Authority,
                                    ClientId = notification.Options.ClientId,
                                    KeyVaultIdentifier = govUkOidcConfiguration.KeyVaultIdentifier
                                });
                            oidcService.PopulateAccountClaims(notification.AuthenticationTicket.Identity, notification.ProtocolMessage.AccessToken);
                            postAuthenticationHandler.Handle(notification.AuthenticationTicket.Identity);
                            
                            return Task.CompletedTask;
                        }
                    }
                });
            }
            else
            {
                app.UseCodeFlowAuthentication(new OidcMiddlewareOptions {
                    AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                    BaseUrl = oidcConfiguration.BaseAddress,
                    ClientId = oidcConfiguration.ClientId,
                    ClientSecret = oidcConfiguration.ClientSecret,
                    Scopes = oidcConfiguration.Scopes,
                    AuthorizeEndpoint = authenticationUrls.AuthorizeEndpoint,
                    TokenEndpoint = authenticationUrls.TokenEndpoint,
                    UserInfoEndpoint = authenticationUrls.UserInfoEndpoint,
                    TokenSigningCertificateLoader = GetSigningCertificate(oidcConfiguration.UseCertificate, false, oidcConfiguration.TokenCertificateThumbprint),
                    TokenValidationMethod = oidcConfiguration.UseCertificate ? TokenValidationMethod.SigningKey : TokenValidationMethod.BinarySecret,
                    AuthenticatedCallback = i => postAuthenticationHandler.Handle(i)
                });

                ConfigurationFactory.Current = new IdentityServerConfigurationFactory(oidcConfiguration);
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();
            }
        }

        private Func<X509Certificate2> GetSigningCertificate(bool useCertificate, bool isDevEnvironment, string certThumbprint)
        {
            if (!useCertificate)
            {
                return null;
            }

            return () =>
            {
                var storeLocation = isDevEnvironment ? StoreLocation.LocalMachine : StoreLocation.CurrentUser;
                var store = new X509Store(StoreName.My, storeLocation);
                store.Open(OpenFlags.ReadOnly);
                try
                {
                    var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, certThumbprint, false);

                    if (certificates.Count < 1)
                    {
                        throw new Exception($"Could not find certificate with thumbprint {certThumbprint} in CurrentUser store");
                    }

                    return certificates[0];
                }
                finally
                {
                    store.Close();
                }
            };
        }
    }
}