using System.Web.Http;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.DependencyResolution;
using SFA.DAS.UnitOfWork.EntityFrameworkCore;
using SFA.DAS.UnitOfWork.SqlServer;
using WebApi.StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(HttpConfiguration config)
        {
            config.UseStructureMap(c =>
            {
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<AuthenticationRegistry>();
                c.AddRegistry<ApiDataRegistry>();
                c.AddRegistry<EntityFrameworkCoreUnitOfWorkRegistry<ProviderRelationshipsDbContext>>();
                c.AddRegistry<HashingRegistry>();
                c.AddRegistry<LoggerRegistry>();
                c.AddRegistry<MapperRegistry>();
                c.AddRegistry<MediatorRegistry>();
                c.AddRegistry<SqlServerUnitOfWorkRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}