using SFA.DAS.ProviderRelationships.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Web.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<LoggerRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}