using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Startup;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Jobs.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IStartupTask>().Add<StartupEndpoint>();
            For<ProviderRelationshipsDbContext>().Use(c => GetDbContext(c));
        }

        private ProviderRelationshipsDbContext GetDbContext(IContext context)
        {
            var connectionString = context.GetInstance<ProviderRelationshipsConfiguration>().DatabaseConnectionString;
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseSqlServer(connectionString);
            var db = new ProviderRelationshipsDbContext(optionsBuilder.Options);
            
            return db;
        }
    }
}