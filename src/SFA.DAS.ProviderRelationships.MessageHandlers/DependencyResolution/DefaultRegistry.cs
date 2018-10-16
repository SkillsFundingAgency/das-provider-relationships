using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IStartupTask>().Add<StartupEndpoint>();
        }
    }
}