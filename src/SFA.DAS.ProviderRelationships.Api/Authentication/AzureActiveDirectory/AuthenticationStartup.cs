using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Authentication.AzureActiveDirectory;

namespace SFA.DAS.ProviderRelationships.Api.Authentication.AzureActiveDirectory
{
    public class AuthenticationStartup : IAuthenticationStartup
    {
        private readonly IAppBuilder _app;
        private readonly IAzureActiveDirectoryConfiguration _config;
        private readonly ILog _log;
        
        public AuthenticationStartup(
            IAppBuilder app,
            IAzureActiveDirectoryConfiguration config,
            ILog log)
        {
            _app = app;
            _config = config;
            _log = log;
        }

        public void Initialise()
        {
            _log.Info($"Initializing Azure AD Bearer Authentication with Tenant '{_config.Tenant}', Audience '{_config.Audience}'");

            _app.UseWindowsAzureActiveDirectoryBearerAuthentication(new WindowsAzureActiveDirectoryBearerAuthenticationOptions
            {
                Tenant = _config.Tenant,
                TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    ValidAudience = _config.Audience
                }
            });
        }
    }
}