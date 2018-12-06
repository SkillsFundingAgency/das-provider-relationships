using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class DefaultTransactionDbContextFactory : IProviderRelationshipsDbContextFactory
    {
        private readonly ProviderRelationshipsConfiguration _providerRelationshipsConfiguration;
        private readonly ILoggerFactory _loggerFactory;

        public DefaultTransactionDbContextFactory(ProviderRelationshipsConfiguration providerRelationshipsConfiguration, ILoggerFactory loggerFactory)
        {
            _providerRelationshipsConfiguration = providerRelationshipsConfiguration;
            _loggerFactory = loggerFactory;
        }

        public ProviderRelationshipsDbContext CreateDbContext()
        {
            var sqlConnection = new SqlConnection(_providerRelationshipsConfiguration.DatabaseConnectionString);
            
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseLoggerFactory(_loggerFactory)
                .UseSqlServer(sqlConnection)
                .ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));
            
            return new ProviderRelationshipsDbContext(optionsBuilder.Options);
        }
    }
}