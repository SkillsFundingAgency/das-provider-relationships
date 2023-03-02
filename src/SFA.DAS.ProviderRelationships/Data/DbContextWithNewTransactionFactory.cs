using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFA.DAS.AutoConfiguration;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class DbContextWithNewTransactionFactory : IProviderRelationshipsDbContextFactory
    {
        private readonly DbConnection _dbConnection;
        private readonly IEnvironmentService _environmentService;
        private readonly ILoggerFactory _loggerFactory;

        public DbContextWithNewTransactionFactory(DbConnection dbConnection, IEnvironmentService environmentService, ILoggerFactory loggerFactory)
        {
            _dbConnection = dbConnection;
            _environmentService = environmentService;
            _loggerFactory = loggerFactory;
        }

        public ProviderRelationshipsDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseSqlServer(_dbConnection);

            if (_environmentService.IsCurrent(DasEnv.LOCAL))
            {
                optionsBuilder.UseLoggerFactory(_loggerFactory);
            }
            
            var dbContext = new ProviderRelationshipsDbContext(optionsBuilder.Options);

            return dbContext;
        }
    }
}