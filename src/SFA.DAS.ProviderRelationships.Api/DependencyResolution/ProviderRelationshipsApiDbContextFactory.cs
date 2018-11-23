using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.DependencyResolution;

namespace SFA.DAS.ProviderRelationships.Api.DependencyResolution
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
            //todo: new factory for api. remove nservice bus, inject in config, create connection. remove usetransaction

            //todo: inject container and locate DbConnection instead?
            var sqlConnection = new SqlConnection(_providerRelationshipsConfiguration.DatabaseConnectionString);
            
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseLoggerFactory(_loggerFactory)
                .UseSqlServer(sqlConnection)
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            return new ProviderRelationshipsDbContext(optionsBuilder.Options);
        }
    }
}