using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Api.Data
{
    public class ProviderRelationshipsApiDbContextFactory : IProviderRelationshipsDbContextFactory
    {
        private readonly ProviderRelationshipsConfiguration _providerRelationshipsConfiguration;
        private readonly ILoggerFactory _loggerFactory;

        public ProviderRelationshipsApiDbContextFactory(ProviderRelationshipsConfiguration providerRelationshipsConfiguration, ILoggerFactory loggerFactory)
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
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            return new ProviderRelationshipsDbContext(optionsBuilder.Options);
        }
    }
}