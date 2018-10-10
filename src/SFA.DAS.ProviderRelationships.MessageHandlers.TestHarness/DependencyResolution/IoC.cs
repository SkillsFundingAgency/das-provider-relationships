using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.DependencyResolution;
using SFA.DAS.UnitOfWork.EntityFramework;
using SFA.DAS.UnitOfWork.NServiceBus;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<DataRegistry>();
                //todo: interface
                c.AddRegistry<EntityFrameworkUnitOfWorkRegistry<ProviderRelationshipsDbContext>>();
                c.AddRegistry<LoggerRegistry>();
                c.AddRegistry<MapperRegistry>();
                c.AddRegistry<MediatorRegistry>();
                c.AddRegistry<NServiceBusUnitOfWorkRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}