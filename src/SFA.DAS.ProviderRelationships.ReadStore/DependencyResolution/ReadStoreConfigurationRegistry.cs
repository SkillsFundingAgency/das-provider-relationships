using SFA.DAS.AutoConfiguration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.ReadStore.DependencyResolution
{
    public class ReadStoreConfigurationRegistry : Registry
    {
        public ReadStoreConfigurationRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();
            For<ProviderRelationshipsReadStoreConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>().Get<ProviderRelationshipsReadStoreConfiguration>());
        }
    }
}
