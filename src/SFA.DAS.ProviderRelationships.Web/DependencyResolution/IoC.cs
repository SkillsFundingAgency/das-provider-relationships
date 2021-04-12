using StructureMap;
using SFA.DAS.Authorization;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.StructureMap;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.DependencyResolution;
using SFA.DAS.UnitOfWork.EntityFrameworkCore.DependencyResolution.StructureMap;
using SFA.DAS.UnitOfWork.NServiceBus.DependencyResolution.StructureMap;
using SFA.DAS.UnitOfWork.NServiceBus.Features.ClientOutbox.DependencyResolution.StructureMap;

namespace SFA.DAS.ProviderRelationships.Web.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<AuthorizationRegistry>();
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<EmployerFeaturesAuthorizationRegistry>();
                c.AddRegistry<EmployerUserRolesAuthorizationRegistry>();
                c.AddRegistry<EntityFrameworkCoreUnitOfWorkRegistry<ProviderRelationshipsDbContext>>();
                c.AddRegistry<HashingRegistry>();
                c.AddRegistry<LoggerRegistry>();
                c.AddRegistry<MapperRegistry>();
                c.AddRegistry<MediatorRegistry>();
                c.AddRegistry<NServiceBusClientUnitOfWorkRegistry>();
                c.AddRegistry<NServiceBusUnitOfWorkRegistry>();
                c.AddRegistry<OidcAuthenticationRegistry>();
                c.AddRegistry<ProviderRelationshipsApiClientRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<PasAccountApiRegistry>();
                c.AddRegistry<RecruitApiRegistry>();
                c.AddRegistry<RegistrationApiRegistry>();
                c.AddRegistry<DefaultRegistry>();
                c.AddRegistry<RoatpApiRegistry>();
            });
        }
    }
}