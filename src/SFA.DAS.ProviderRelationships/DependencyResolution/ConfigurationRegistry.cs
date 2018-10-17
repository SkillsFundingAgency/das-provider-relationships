using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            For<ProviderRelationshipsConfiguration>().Use(() => ConfigurationHelper.GetConfiguration<ProviderRelationshipsConfiguration>("SFA.DAS.ProviderRelationships").InitialTransform()).Singleton();
            For<IIdentityServerConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().Identity).Singleton();
            For<IStartup>().Use<Startup>().Singleton();
        }
    }
}