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
using SFA.DAS.ProviderRelationships.Authentication.Interfaces;
using SFA.DAS.ProviderRelationships.Configuration;

//todo: signout. need full signout between ma/ec/pr
// can we access cookies of parent domain to delete them? can the tld store cookies in subdomain? can we delegate all cookie deleted to ma (can tld create subdomain cookies)
// can we round robin around the websites to give them all a chance to signout. we could have a set path through the signouts and terminate it when site is already signed out
// e.g. ma returns to empcom, empcom returns to prorel, prorel returns to ma, then it doens't matter which signout gets called first

namespace SFA.DAS.ProviderRelationships.Authentication
{
    //todo: this code is boilerplate code. do we want to package it somehow and make it easier to reuse?
    public class AuthenticationStartup : IAuthenticationStartup
    {
        private readonly IAppBuilder _app;
        private readonly IIdentityServerConfiguration _config;
        private readonly IAuthenticationUrls _authenticationUrls;
        private readonly IClaimValue _claimValues;
        private readonly ILog _logger;

        public AuthenticationStartup(
            IAppBuilder app,
            IIdentityServerConfiguration config,
            IAuthenticationUrls authenticationUrls,
            IClaimValue claimValues,
            ILog logger)
        {
            _app = app;
            _config = config;
            _authenticationUrls = authenticationUrls;
            _claimValues = claimValues;
            _logger = logger;
        }

        public void Initialise()
        {
            _logger.Info("Initialising Authentication");

            //todo: should we DI everything we new up? e.g. IdentityServerConfigurationFactory?

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
                AuthorizeEndpoint = _authenticationUrls.AuthorizeEndpoint,
                TokenEndpoint = _authenticationUrls.TokenEndpoint,
                UserInfoEndpoint = _authenticationUrls.UserInfoEndpoint,
                TokenSigningCertificateLoader = GetSigningCertificate(_config.UseCertificate),
                TokenValidationMethod = _config.UseCertificate
                    ? TokenValidationMethod.SigningKey
                    : TokenValidationMethod.BinarySecret,
                AuthenticatedCallback = identity => PostAuthenticationAction(identity, _claimValues)
            });

            ConfigurationFactory.Current = new IdentityServerConfigurationFactory(_config);
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
        }

        internal Func<X509Certificate2> GetSigningCertificate(bool useCertificate)
        {
            if (!useCertificate)
            {
                _logger.Info("Not using certificate");
                return null;
            }

            return () =>
            {
                // we need to use CurrentUser as we'll be hosted as an app service
                // https://docs.microsoft.com/en-us/azure/app-service/app-service-web-ssl-cert-load
                var store = new X509Store(StoreLocation.CurrentUser);

                store.Open(OpenFlags.ReadOnly);

                try
                {
                    //todo: add config
                    var thumbprint = CloudConfigurationManager.GetSetting("TokenCertificateThumbprint");
                    var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                    if (certificates.Count < 1)
                        throw new Exception($"Could not find certificate with thumbprint '{thumbprint}' in CurrentUser store");

                    _logger.Info($"Found and using certificate with thumbprint '{thumbprint}' in CurrentUser store");

                    return certificates[0];
                }
                finally
                {
                    store.Close();
                }
            };
        }

        internal void PostAuthenticationAction(ClaimsIdentity identity, IClaimValue claimValue)
        {
            _logger.Info("Retrieving claims from OIDC server");

            var userRef = identity.GetClaimValue(claimValue.Id);
            var email = identity.GetClaimValue(claimValue.Email);
            var displayName = identity.GetClaimValue(claimValue.DisplayName);

            // these claims should be there, but we don't need them:
            // claimValue.GivenName (firstname), claimValue.FamilyName (lastname)

            if (userRef == null || email == null || displayName == null) // checked with Ben (security team) that it's ok to log these details when something has gone wrong
                throw new Exception($"Missing claim '{userRef ?? "null"}', '{email ?? "null"}', '{displayName ?? "null"}'");

            // we don't store personally identifiable info in the logs for security purposes
            // so we'll have to consistently log the userRef elsewhere, and for support we might have to look up the user details from EAS's user db
            _logger.Info($"Claims retrieved from OIDC server for user '{userRef}'");

            //todo: why are we adding claims where we already have claims with the same value?
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userRef));
            identity.AddClaim(new Claim(ClaimTypes.Name, displayName));
            identity.AddClaim(new Claim("sub", userRef));
            identity.AddClaim(new Claim("email", email));
        }
    }
}
