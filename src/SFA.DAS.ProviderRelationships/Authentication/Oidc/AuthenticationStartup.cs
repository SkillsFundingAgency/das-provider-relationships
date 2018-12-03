using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.NLog.Logger;
using SFA.DAS.OidcMiddleware;

namespace SFA.DAS.ProviderRelationships.Authentication.Oidc
{
    /// <remarks>
    /// Works with DAS' implementation of OpenID Connect Provider (https://github.com/SkillsFundingAgency/das-employerusers)
    /// which uses/implements...
    /// SFA.DAS.OidcMiddleware (https://github.com/SkillsFundingAgency/das-shared-packages)
    /// IdentityServer3 (https://github.com/IdentityServer/IdentityServer3)
    /// OpenID Connect (https://openid.net/specs/openid-connect-core-1_0.html#toc)
    /// OAuth2 (https://tools.ietf.org/html/rfc6749)
    /// Backgrounder...
    /// https://developer.okta.com/blog/2017/07/25/oidc-primer-part-1
    /// </remarks>>
    public class AuthenticationStartup : IAuthenticationStartup
    {
        private readonly IAppBuilder _app;
        private readonly IOidcConfiguration _config;
        private readonly IAuthenticationUrls _authenticationUrls;
        private readonly IPostAuthenticationHandler _postAuthenticationHandler;
        private readonly ConfigurationFactory _configurationFactory;
        private readonly ILog _logger;

        public AuthenticationStartup(
            IAppBuilder app,
            IOidcConfiguration config,
            IAuthenticationUrls authenticationUrls,
            IPostAuthenticationHandler postAuthenticationHandler,
            ConfigurationFactory configurationFactory,
            ILog logger)
        {
            _app = app;
            _config = config;
            _authenticationUrls = authenticationUrls;
            _postAuthenticationHandler = postAuthenticationHandler;
            _configurationFactory = configurationFactory;
            _logger = logger;
        }

        public void Initialise()
        {
            _logger.Info("Initialising Authentication");

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
                TokenValidationMethod = _config.UseCertificate ? TokenValidationMethod.SigningKey : TokenValidationMethod.BinarySecret,
                AuthenticatedCallback = i => _postAuthenticationHandler.Handle(i)
            });

            ConfigurationFactory.Current = _configurationFactory;
            
            // replace MS's default claim mapping (between OIDC claims and MS .net proprietary claim types)
            // with an empty mapper - the claim types we receive are DAS specific
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
        }

        private Func<X509Certificate2> GetSigningCertificate(bool useCertificate)
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
                    var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, _config.TokenCertificateThumbprint, false);

                    if (certificates.Count < 1)
                        throw new Exception($"Could not find certificate with thumbprint '{_config.TokenCertificateThumbprint}' in CurrentUser store");

                    _logger.Info($"Found and using certificate with thumbprint '{_config.TokenCertificateThumbprint}' in CurrentUser store");

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