using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.ReadStore.DependencyResolution
{
    public class ReadStoreConfigurationRegistry : Registry
    {
        public ReadStoreConfigurationRegistry()
        {
            For<ProviderRelationshipsReadStoreConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>().Get<ProviderRelationshipsReadStoreConfiguration>());
        }
    }
}
