using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class DbContextWithNewTransactionFactory : IProviderRelationshipsDbContextFactory
    {
        private readonly DbConnection _dbConnection;
        private readonly ILoggerFactory _loggerFactory;

        public DbContextWithNewTransactionFactory(DbConnection dbConnection, ILoggerFactory loggerFactory)
        {
            _dbConnection = dbConnection;
            _loggerFactory = loggerFactory;
        }

        public ProviderRelationshipsDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseLoggerFactory(_loggerFactory)
                .UseSqlServer(_dbConnection)
                .ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));
            
            var dbContext = new ProviderRelationshipsDbContext(optionsBuilder.Options);

            return dbContext;
        }
    }
}