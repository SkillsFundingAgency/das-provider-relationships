using SFA.DAS.ProviderRelationships.Startup;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class StartupRegistry : Registry
    {
        public StartupRegistry()
        {
            Scan(s =>
            {
                s.AssembliesAndExecutablesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("SFA.DAS"));
                s.Convention<CompositeDecorator<DefaultStartup, IStartup>>();
            });
        }
    }
}