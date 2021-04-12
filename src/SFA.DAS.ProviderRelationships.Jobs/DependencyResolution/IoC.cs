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
                c.AddRegistry<RoatpApiRegistry>();
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<LoggerRegistry>();
                c.AddRegistry<ReadStoreDataRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}