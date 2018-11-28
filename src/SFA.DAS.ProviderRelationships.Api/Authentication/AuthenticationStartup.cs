using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.Authentication
{
    public class AuthenticationStartup : IAuthenticationStartup
    {
        private readonly IAppBuilder _app;
        private readonly ProviderRelationshipsConfiguration _providerRelationshipsConfiguration;
        private readonly ILog _log;
        
        public AuthenticationStartup(IAppBuilder app,
            ProviderRelationshipsConfiguration providerRelationshipsConfiguration,
            ILog log)
        {
            _app = app;
            _providerRelationshipsConfiguration = providerRelationshipsConfiguration;
            _log = log;
        }

        public void Initialize()
        {
            _log.Info($"Initializing Azure AD Bearer Authentication with Tenant:{_providerRelationshipsConfiguration.Tenant}, Audience:{_providerRelationshipsConfiguration.Audience}");

            _app.UseWindowsAzureActiveDirectoryBearerAuthentication(new WindowsAzureActiveDirectoryBearerAuthenticationOptions
            {
                Tenant = _providerRelationshipsConfiguration.Tenant,
                TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    ValidAudience = _providerRelationshipsConfiguration.Audience
                }
            });
        }
    }
}
