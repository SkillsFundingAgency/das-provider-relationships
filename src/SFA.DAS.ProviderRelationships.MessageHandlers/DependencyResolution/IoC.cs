﻿using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.DependencyResolution;
using SFA.DAS.UnitOfWork.EntityFrameworkCore.DependencyResolution.StructureMap;
using SFA.DAS.UnitOfWork.NServiceBus.DependencyResolution.StructureMap;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<EntityFrameworkCoreUnitOfWorkRegistry<ProviderRelationshipsDbContext>>();
                c.AddRegistry<LoggerRegistry>();
                c.AddRegistry<MapperRegistry>();
                c.AddRegistry<MediatorRegistry>();
                c.AddRegistry<NServiceBusUnitOfWorkRegistry>();
                c.AddRegistry<ReadStoreDataRegistry>();
                c.AddRegistry<PasAccountApiRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}