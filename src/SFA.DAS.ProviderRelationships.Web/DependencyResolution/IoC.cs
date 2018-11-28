using SFA.DAS.Authorization;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerRoles;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.DependencyResolution;
using SFA.DAS.UnitOfWork.EntityFrameworkCore;
using SFA.DAS.UnitOfWork.NServiceBus;
using SFA.DAS.UnitOfWork.NServiceBus.ClientOutbox;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Web.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<ApprenticeshipInfoServiceApiRegistry>();
                c.AddRegistry<AuthenticationRegistry>();
                c.AddRegistry<AuthorizationRegistry>();
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<EmployerFeaturesAuthorizationRegistry>();
                c.AddRegistry<EmployerRolesAuthorizationRegistry>();
                c.AddRegistry<EntityFrameworkCoreUnitOfWorkRegistry<ProviderRelationshipsDbContext>>();
                c.AddRegistry<HashingRegistry>();
                c.AddRegistry<LoggerRegistry>();
                c.AddRegistry<MapperRegistry>();
                c.AddRegistry<MediatorRegistry>();
                c.AddRegistry<NServiceBusClientUnitOfWorkRegistry>();
                c.AddRegistry<NServiceBusUnitOfWorkRegistry>();
                c.AddRegistry<ProviderRelationshipsApiClientRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<UrlsRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}