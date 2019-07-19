using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.StructureMap
{
    internal class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            For<AzureActiveDirectoryClientConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsApiClientConfiguration>().AzureActiveDirectoryClient).Singleton();
            For<ProviderRelationshipsApiClientConfiguration>().Use(c => c.GetInstance<IAutoConfigurationService>().Get<ProviderRelationshipsApiClientConfiguration>(ConfigurationKeys.ApiClientBase)).Singleton();
            For<ReadStoreConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsApiClientConfiguration>().ReadStore).Singleton();
        }
    }
}