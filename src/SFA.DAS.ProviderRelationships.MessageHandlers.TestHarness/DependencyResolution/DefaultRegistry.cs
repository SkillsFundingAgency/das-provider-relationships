using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IStartupTask>().Add<StartupEndpoint>();
        }
    }
}