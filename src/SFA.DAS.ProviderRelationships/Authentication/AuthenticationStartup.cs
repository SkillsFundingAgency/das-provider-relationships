using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.NLog.Logger;
using SFA.DAS.OidcMiddleware;
using SFA.DAS.ProviderRelationships.Configuration;

//todo: signout. need full signout between ma/ec/pr
// can we access cookies of parent domain to delete them? can the tld store cookies in subdomain? can we delegate all cookie deleted to ma (can tld create subdomain cookies)
// can we round robin around the websites to give them all a chance to signout. we could have a set path through the signouts and terminate it when site is already signed out
// e.g. ma returns to empcom, empcom returns to prorel, prorel returns to ma, then it doens't matter which signout gets called first

namespace SFA.DAS.ProviderRelationships.Authentication
{
    //todo: better name for this?
    //todo: this code is boilerplate code. do we want to package it somehow and make it easier to reuse?
    public class AuthenticationStartup : IAuthenticationStartup
    {
        private readonly IAppBuilder _app;
        private readonly IIdentityServerConfiguration _config;
        private readonly ILog _logger;

        public AuthenticationStartup(IAppBuilder app, IIdentityServerConfiguration config, ILog logger)
        {
            _app = app;
            _config = config;
            _logger = logger;
        }

        public void Initialise()
        {
            _logger.Info("Initialising Authentication");

            var authenticationUrls = new AuthenticationUrls(_config);
            var claimValues = new ClaimValue(_config.ClaimIdentifierConfiguration);

            _app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                ExpireTimeSpan = new TimeSpan(0, 10, 0),
                SlidingExpiration = true
            });

            _app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "TempState",
                AuthenticationMode = AuthenticationMode.Passive
            });

            _app.UseCodeFlowAuthentication(new OidcMiddlewareOptions
            {
                BaseUrl = _config.BaseAddress,
                ClientId = _config.ClientId,
                ClientSecret = _config.ClientSecret,
                Scopes = _config.Scopes,
                AuthorizeEndpoint = authenticationUrls.AuthorizeEndpoint,
                TokenEndpoint = authenticationUrls.TokenEndpoint,
                UserInfoEndpoint = authenticationUrls.UserInfoEndpoint,
                TokenSigningCertificateLoader = GetSigningCertificate(_config.UseCertificate),
                TokenValidationMethod = _config.UseCertificate
                    ? TokenValidationMethod.SigningKey
                    : TokenValidationMethod.BinarySecret,
                AuthenticatedCallback = identity => { PostAuthenticationAction(identity, claimValues, _logger); }
            });

            ConfigurationFactory.Current = new IdentityServerConfigurationFactory(_config);
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
        }

        private static Func<X509Certificate2> GetSigningCertificate(bool useCertificate)
        {
            if (!useCertificate)
                return null;

            return () =>
            {
                //todo: we need to use CurrentUser as we'll be hosted as an app service
                // EAS uses CurrentUser, EmpCom uses LocalMachine (which is where our config'ed cert is stored)
                var store = new X509Store(StoreLocation.LocalMachine);

                store.Open(OpenFlags.ReadOnly);

                try
                {
                    //todo: add config
                    var thumbprint = CloudConfigurationManager.GetSetting("TokenCertificateThumbprint");
                    var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                    if (certificates.Count < 1)
                        throw new Exception($"Could not find certificate with thumbprint '{thumbprint}' in LocalMachine store.");

                    return certificates[0];
                }
                finally
                {
                    store.Close();
                }
            };
        }

        //todo: need test coverage of this
        //todo: does this need to be static?
        private static void PostAuthenticationAction(ClaimsIdentity identity, ClaimValue claimValue, ILog logger)
        {
            logger.Info("Retrieving claims from OIDC server");

            var userRef = identity.GetClaimValue(claimValue.Id);
            var email = identity.GetClaimValue(claimValue.Email);
            var displayName = identity.GetClaimValue(claimValue.DisplayName);

            // these claims should be there, but we don't need them:
            // claimValue.GivenName (firstname), claimValue.FamilyName (lastname)

            if (userRef == null || email == null || displayName == null) // checked with Ben (security team) that it's ok to log these details when something has gone wrong
                throw new Exception($"Missing claim '{userRef ?? "null"}', '{email ?? "null"}', '{displayName ?? "null"}'");

            // we don't store personally identifiable info in the logs for security purposes
            // so we'll have to consistently log the userRef elsewhere, and for support we might have to look up the user details from EAS's user db
            logger.Info($"Claims retrieved from OIDC server for user '{userRef}'");

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userRef));
            identity.AddClaim(new Claim(ClaimTypes.Name, displayName));
            //todo: do we need to create 2 claims containing userRef?
            identity.AddClaim(new Claim("sub", userRef));
            identity.AddClaim(new Claim("email", email));
        }
    }
}
