using System.Collections.Specialized;
using System.Configuration;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            For<ITableStorageConfigurationService>().Use<TableStorageConfigurationService>();
            For<EmployerFeaturesConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>().Get<EmployerFeaturesConfiguration>("SFA.DAS.ProviderRelationships.EmployerFeatures")).Singleton();
            For<ProviderRelationshipsConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>().Get<ProviderRelationshipsConfiguration>("SFA.DAS.ProviderRelationships")).Singleton();
        }
    }
}