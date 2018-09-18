//using SFA.DAS.EmployerUsers.WebClientComponents;
//using SFA.DAS.ProviderRelationships.Configuration;

//namespace SFA.DAS.ProviderRelationships.Web.Authentication
//{
//    internal class IdentityServerConfigurationFactory : ConfigurationFactory
//    {
//        private readonly ProviderRelationshipsConfiguration _configuration;

//        public IdentityServerConfigurationFactory(ProviderRelationshipsConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public override ConfigurationContext Get()
//        {
//            return new ConfigurationContext
//            {
//                //todo: how often does this get called? can we calc this in the ctor (& use interpolation) - looks like it won't change
//                AccountActivationUrl = _configuration.Identity.BaseAddress.Replace("/identity", "") + _configuration.Identity.AccountActivationUrl
//            };
//        }
//    }
//}