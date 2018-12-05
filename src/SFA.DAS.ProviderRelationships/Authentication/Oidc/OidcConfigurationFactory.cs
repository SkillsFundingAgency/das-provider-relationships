﻿using SFA.DAS.EmployerUsers.WebClientComponents;

namespace SFA.DAS.ProviderRelationships.Authentication.Oidc
{
    public class OidcConfigurationFactory : ConfigurationFactory
    {
        private readonly IOidcConfiguration _config;

        public OidcConfigurationFactory(IOidcConfiguration config)
        {
            _config = config;
        }

        public override ConfigurationContext Get()
        {
            return new ConfigurationContext
            {
                //todo: how often does this get called? can we calc this in the ctor - looks like it won't change
                // this doesn't seem to get called at all during signin
                AccountActivationUrl = $"{_config.BaseAddress.Replace("/identity", "")}{_config.AccountActivationUrl}"
            };
        }
    }
}