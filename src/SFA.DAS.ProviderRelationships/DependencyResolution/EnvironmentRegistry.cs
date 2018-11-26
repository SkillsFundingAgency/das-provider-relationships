using SFA.DAS.AutoConfiguration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class EnvironmentRegistry : Registry
    {
        public EnvironmentRegistry()
        {
            For<IEnvironmentService>().Use<EnvironmentService>();
        }
    }
}