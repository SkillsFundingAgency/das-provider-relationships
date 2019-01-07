using StructureMap;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    internal class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            For<ProviderRelationshipsApiClientConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>()
                .Get<ProviderRelationshipsApiClientConfiguration>(ConfigurationNames.ApiClientBase)).Singleton();
            For<ReadStoreConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsApiClientConfiguration>().ReadStore).Singleton();
            For<AzureActiveDirectoryClientConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsApiClientConfiguration>().AzureActiveDirectoryClient).Singleton();
        }
    }
}