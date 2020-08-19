using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public class IdentityServerConfigurationFactory : ConfigurationFactory
    {
        private readonly IOidcConfiguration _configuration;

        public IdentityServerConfigurationFactory(IOidcConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override ConfigurationContext Get()
        {
            return new ConfigurationContext {
                AccountActivationUrl = _configuration.BaseAddress.Replace("/identity", "") + _configuration.AccountActivationUrl
            };
        }
    }
}