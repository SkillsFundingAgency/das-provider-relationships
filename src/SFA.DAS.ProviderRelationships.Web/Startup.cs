using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using NLog;
using Owin;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.OidcMiddleware;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web;
using SFA.DAS.ProviderRelationships.Web.App_Start;
using SFA.DAS.ProviderRelationships.Web.Authentication;

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

            if (providerRelationshipsConfiguration is { UseGovUKSignIn: true })//this is a nasty hack due to use of old ver of shared feature toggle lib
            {
                // gov.uk stuff here., (and will require redirect to https port 44363)                
            }
            else
            {
                
            }

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
                    var thumbprint = certThumbprint;
                    var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                    if (certificates.Count < 1)
                    {
                        throw new Exception($"Could not find certificate with thumbprint {thumbprint} in CurrentUser store");
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