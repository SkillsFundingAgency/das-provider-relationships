using System.Collections.Specialized;
using System.Configuration;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            // belongs here, or in EnvironmentRegistry?
            For<IEnvironment>().Use<Environment>()
                .Ctor<NameValueCollection>().Is(ConfigurationManager.AppSettings).Singleton();

            For<IEnvironmentConfiguration>().Use<EnvironmentConfiguration>()
                .Ctor<NameValueCollection>().Is(ConfigurationManager.AppSettings);

            For<ProviderRelationshipsConfiguration>().Use(c => c.GetInstance<IEnvironmentConfiguration>().Get<ProviderRelationshipsConfiguration>("SFA.DAS.ProviderRelationships").InitialTransform()).Singleton();
            For<ProviderRelationshipsReadStoreConfiguration>().Use(c => c.GetInstance<IEnvironmentConfiguration>().Get<ProviderRelationshipsReadStoreConfiguration>("SFA.DAS.ProviderRelationships.ReadStore")).Singleton();
        }
    }
}