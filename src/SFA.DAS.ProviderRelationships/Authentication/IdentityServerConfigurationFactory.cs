using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    public class IdentityServerConfigurationFactory : ConfigurationFactory
    {
        private readonly IIdentityServerConfiguration _config;

        public IdentityServerConfigurationFactory(IIdentityServerConfiguration config)
        {
            _config = config;
        }

        public override ConfigurationContext Get()
        {
            return new ConfigurationContext
            {
                //todo: how often does this get called? can we calc this in the ctor (& use interpolation) - looks like it won't change
                AccountActivationUrl = _config.BaseAddress.Replace("/identity", "") + _config.AccountActivationUrl
            };
        }
    }
}