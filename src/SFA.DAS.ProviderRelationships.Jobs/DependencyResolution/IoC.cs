using SFA.DAS.ProviderRelationships.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Jobs.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}