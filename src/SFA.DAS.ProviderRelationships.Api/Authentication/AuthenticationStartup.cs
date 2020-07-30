using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Configuration;
using System.IdentityModel.Tokens;

namespace SFA.DAS.ProviderRelationships.Api.Authentication
{
    public class AuthenticationStartup : IAuthenticationStartup
    {
        private readonly IAppBuilder _app;
        private readonly IAzureActiveDirectoryConfiguration _config;
        private readonly ILog _logger;
        
        public AuthenticationStartup(
            IAppBuilder app,
            IAzureActiveDirectoryConfiguration config,
            ILog logger)
        {
            _app = app;
            _config = config;
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.Info($"Initializing Azure Active Directory Bearer Authentication with Tenant '{_config.Tenant}', Audience '{_config.Audience}'");
            
            _app.UseWindowsAzureActiveDirectoryBearerAuthentication(new WindowsAzureActiveDirectoryBearerAuthenticationOptions
            {
                Tenant = _config.Tenant,
                TokenValidationParameters = new TokenValidationParameters {

                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    ValidAudience = _config.Audience
                }
            });
        }
    }
}