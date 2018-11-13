using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            For<EmployerFeaturesConfiguration>().Use(() => ConfigurationHelper.GetConfiguration<EmployerFeaturesConfiguration>("SFA.DAS.ProviderRelationships.EmployerFeatures")).Singleton();
            For<ProviderRelationshipsConfiguration>().Use(() => ConfigurationHelper.GetConfiguration<ProviderRelationshipsConfiguration>("SFA.DAS.ProviderRelationships").InitialTransform()).Singleton();
            For<ProviderRelationshipsReadStoreConfiguration>().Use(() => ConfigurationHelper.GetConfiguration<ProviderRelationshipsReadStoreConfiguration>("SFA.DAS.ProviderRelationships.ReadStore")).Singleton();
        }
    }
}