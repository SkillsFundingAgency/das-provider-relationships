using SFA.DAS.ProviderRelationships.Startup;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class StartupRegistry : Registry
    {
        public StartupRegistry()
        {
            For<IStartup>().Use<Startup.Startup>().Singleton();
        }
    }
}