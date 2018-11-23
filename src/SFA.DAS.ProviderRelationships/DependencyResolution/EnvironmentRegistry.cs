using System.Collections.Specialized;
using System.Configuration;
using SFA.DAS.ProviderRelationships.Environment;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;
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