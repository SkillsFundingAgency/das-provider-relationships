//todo: delete?
using SFA.DAS.AutoConfiguration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
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
