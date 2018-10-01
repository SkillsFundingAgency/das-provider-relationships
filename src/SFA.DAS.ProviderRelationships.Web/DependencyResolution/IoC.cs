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
                c.AddRegistry<ApprenticeshipInfoServiceApiConfigurationRegistry>();
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<AuthenticationRegistry>();
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<LoggerRegistry>();
                c.AddRegistry<MapperRegistry>();
                c.AddRegistry<MediatorRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}