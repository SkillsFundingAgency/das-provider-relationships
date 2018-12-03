using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.DependencyResolution
{
    public class HarnessDataRegistry : Registry
    {
        public HarnessDataRegistry()
        {
            For<DbConnection>().Use(c => new SqlConnection(c.GetInstance<ProviderRelationshipsConfiguration>().DatabaseConnectionString));
            For<ProviderRelationshipsDbContext>().Use(c => GetDbContext(c));
        }

        private ProviderRelationshipsDbContext GetDbContext(IContext context)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseSqlServer(context.GetInstance<DbConnection>())
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            
            return new ProviderRelationshipsDbContext(optionsBuilder.Options);
        }
    }
}