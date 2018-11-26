using Microsoft.Azure;
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

        //todo: add idaTenant & idaAudience to das-employer-config
        
        public void Initialize()
        {
            _app.UseWindowsAzureActiveDirectoryBearerAuthentication(new WindowsAzureActiveDirectoryBearerAuthenticationOptions
            {
                Tenant = _providerRelationshipsConfiguration.idaTenant,
                //todo: EAS uses TokenValidationParameters from System.IdentityModel.Tokens.Jwt package
//                TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/Migrating-from-Katana-(OWIN)-3.x-to-4.x
                TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    ValidAudience = _providerRelationshipsConfiguration.idaAudience
                }
            });
        }
    }
}
