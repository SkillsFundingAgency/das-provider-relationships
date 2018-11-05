using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Jobs.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
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