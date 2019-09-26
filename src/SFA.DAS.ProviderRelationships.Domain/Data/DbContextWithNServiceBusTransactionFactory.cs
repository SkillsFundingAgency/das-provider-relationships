using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using NServiceBus.Persistence;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.NServiceBus.SqlServer;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.Domain.Data
{
    public class DbContextWithNServiceBusTransactionFactory : IProviderRelationshipsDbContextFactory
    {
        private readonly IEnvironmentService _environmentService;
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly ILoggerFactory _loggerFactory;

        public DbContextWithNServiceBusTransactionFactory(IEnvironmentService environmentService, IUnitOfWorkContext unitOfWorkContext, ILoggerFactory loggerFactory)
        {
            _environmentService = environmentService;
            _unitOfWorkContext = unitOfWorkContext;
            _loggerFactory = loggerFactory;
        }

        public ProviderRelationshipsDbContext CreateDbContext()
        {
            var synchronizedStorageSession = _unitOfWorkContext.Find<SynchronizedStorageSession>();
            var sqlStorageSession = synchronizedStorageSession.GetSqlStorageSession();
            
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseSqlServer(sqlStorageSession.Connection)
                .ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));

            if (_environmentService.IsCurrent(DasEnv.LOCAL))
            {
                optionsBuilder.UseLoggerFactory(_loggerFactory);
            }
            
            var dbContext = new ProviderRelationshipsDbContext(optionsBuilder.Options);
            
            dbContext.Database.UseTransaction(sqlStorageSession.Transaction);

            return dbContext;
        }
    }
}