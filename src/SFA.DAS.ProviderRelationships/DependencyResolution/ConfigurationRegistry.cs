using System.Collections.Specialized;
using System.Configuration;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Environment;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            // belongs here, or in EnvironmentRegistry?
            For<IEnvironment>().Use<Environment.Environment>()
                .Ctor<NameValueCollection>().Is(ConfigurationManager.AppSettings).Singleton();

            For<IConfiguration>().Use<Configuration.Configuration>()
                .Ctor<NameValueCollection>().Is(ConfigurationManager.AppSettings);

            For<EmployerFeaturesConfiguration>().Use(c => c.GetInstance<IConfiguration>().Get<EmployerFeaturesConfiguration>("SFA.DAS.ProviderRelationships.EmployerFeatures")).Singleton();
            For<ProviderRelationshipsConfiguration>().Use(c => c.GetInstance<IConfiguration>().Get<ProviderRelationshipsConfiguration>("SFA.DAS.ProviderRelationships").InitialTransform()).Singleton();
            For<ProviderRelationshipsReadStoreConfiguration>().Use(c => c.GetInstance<IConfiguration>().Get<ProviderRelationshipsReadStoreConfiguration>("SFA.DAS.ProviderRelationships.ReadStore")).Singleton();
        }
    }
}