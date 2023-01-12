using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using NLog;
using Owin;
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
            var config = container.GetInstance<IOidcConfiguration>();
            var authenticationUrls = container.GetInstance<IAuthenticationUrls>();
            var postAuthenticationHandler = container.GetInstance<IPostAuthenticationHandler>();
            
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

            app.UseCodeFlowAuthentication(new OidcMiddlewareOptions {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                BaseUrl = config.BaseAddress,
                ClientId = config.ClientId,
                ClientSecret = config.ClientSecret,
                Scopes = config.Scopes,
                AuthorizeEndpoint = authenticationUrls.AuthorizeEndpoint,
                TokenEndpoint = authenticationUrls.TokenEndpoint,
                UserInfoEndpoint = authenticationUrls.UserInfoEndpoint,
                TokenSigningCertificateLoader = GetSigningCertificate(config.UseCertificate, false, config.TokenCertificateThumbprint),
                TokenValidationMethod = config.UseCertificate ? TokenValidationMethod.SigningKey : TokenValidationMethod.BinarySecret,
                AuthenticatedCallback = i => postAuthenticationHandler.Handle(i)
            });

            ConfigurationFactory.Current = new IdentityServerConfigurationFactory(config);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();
        }

        private Func<X509Certificate2> GetSigningCertificate(bool useCertificate, bool isDevEnvironement, string certThumbprint)
        {
            if (!useCertificate)
            {
                return null;
            }

            return () =>
            {
                var storeLocation = isDevEnvironement ? StoreLocation.LocalMachine : StoreLocation.CurrentUser;
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