using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using NLog;
using Owin;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.OidcMiddleware;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web;
using SFA.DAS.ProviderRelationships.Web.App_Start;

[assembly: OwinStartup(typeof(Startup))]

namespace SFA.DAS.ProviderRelationships.Web
{
    public class Startup
    {
        //empcom has non-static logger, which should we go with?
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public void Configuration(IAppBuilder app)
        {
            Logger.Info("Starting ProviderRelations Web Application");

            var config = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<ProviderRelationshipsConfiguration>();
            var authenticationUrls = new AuthenticationUrls(config.Identity);
            var claimValues = new ClaimValue(config.Identity.ClaimIdentifierConfiguration);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                ExpireTimeSpan = new TimeSpan(0, 10, 0),
                SlidingExpiration = true
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "TempState",
                AuthenticationMode = AuthenticationMode.Passive
            });

            app.UseCodeFlowAuthentication(new OidcMiddlewareOptions
            {
                BaseUrl = config.Identity.BaseAddress,
                ClientId = config.Identity.ClientId,
                ClientSecret = config.Identity.ClientSecret,
                Scopes = config.Identity.Scopes,
                AuthorizeEndpoint = authenticationUrls.AuthorizeEndpoint,
                TokenEndpoint = authenticationUrls.TokenEndpoint,
                UserInfoEndpoint = authenticationUrls.UserInfoEndpoint,
                TokenSigningCertificateLoader = GetSigningCertificate(config.Identity.UseCertificate),
                TokenValidationMethod = config.Identity.UseCertificate ? TokenValidationMethod.SigningKey : TokenValidationMethod.BinarySecret,
                AuthenticatedCallback = identity =>
                {
                    PostAuthenticationAction(identity, claimValues);
                }
            });

            ConfigurationFactory.Current = new IdentityServerConfigurationFactory(config);
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
        }

        //todo: some of this code is c&p boilerplate code. do we want to package it somehow and make it easier to reuse?
        private static Func<X509Certificate2> GetSigningCertificate(bool useCertificate)
        {
            if (!useCertificate)
                return null;

            return () =>
            {
                //EAS uses..
                //var store = new X509Store(StoreLocation.CurrentUser);
                //EmpCom uses.. (and is where our config'ed cert id stored)
                var store = new X509Store(StoreLocation.LocalMachine);

                store.Open(OpenFlags.ReadOnly);

                try
                {
                    //todo: add config
                    var thumbprint = CloudConfigurationManager.GetSetting("TokenCertificateThumbprint");
                    var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                    if (certificates.Count < 1)
                        throw new Exception($"Could not find certificate with thumbprint '{thumbprint}' in CurrentUser store.");

                    return certificates[0];
                }
                finally
                {
                    store.Close();
                }
            };
        }

        //todo: need test coverage of this
        private static void PostAuthenticationAction(ClaimsIdentity identity, ClaimValue claimValue)
        {
            Logger.Info("Retrieving claims from OIDC server");

            var userRef = identity.GetClaimValue(claimValue.Id);
            var email = identity.GetClaimValue(claimValue.Email);
            var displayName = identity.GetClaimValue(claimValue.DisplayName);

            // these claims should be there, but we don't need them:
            // claimValue.GivenName (firstname), claimValue.FamilyName (lastname)

            if (userRef == null || email == null || displayName == null) // checked with Ben (security team) that it's ok to log these details when something has gone wrong
                throw new Exception($"Missing claim '{userRef ?? "null"}', '{email ?? "null"}', '{displayName ?? "null"}'");

            // we don't store personally identifiable info in the logs for security purposes
            // so we'll have to consistently log the userRef elsewhere, and for support we might have to look up the user details from EAS's user db
            Logger.Info($"Claims retrieved from OIDC server for user '{userRef}'");

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userRef));
            identity.AddClaim(new Claim(ClaimTypes.Name, displayName));
            //todo: do we need to create 2 claims containing userRef?
            identity.AddClaim(new Claim("sub", userRef));
            identity.AddClaim(new Claim("email", email));
        }
    }
}