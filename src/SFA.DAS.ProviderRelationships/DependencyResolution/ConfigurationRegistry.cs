using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            var providerRelationshipsConfig = ConfigurationHelper.GetConfiguration<ProviderRelationshipsConfiguration>("SFA.DAS.ProviderRelationships");

            //interface??
            For<ProviderRelationshipsConfiguration>().Use(() => providerRelationshipsConfig).Singleton();
            For<IIdentityServerConfiguration>().Use(() => providerRelationshipsConfig.Identity).Singleton();
        }
    }
}