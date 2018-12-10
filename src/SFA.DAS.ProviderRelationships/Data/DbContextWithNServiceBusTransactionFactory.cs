using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using NServiceBus.Persistence;
using SFA.DAS.NServiceBus.SqlServer;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class DbContextWithNServiceBusTransactionFactory : IProviderRelationshipsDbContextFactory
    {
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly ILoggerFactory _loggerFactory;

        public DbContextWithNServiceBusTransactionFactory(IUnitOfWorkContext unitOfWorkContext, ILoggerFactory loggerFactory)
        {
            _unitOfWorkContext = unitOfWorkContext;
            _loggerFactory = loggerFactory;
        }

        public ProviderRelationshipsDbContext CreateDbContext()
        {
            var synchronizedStorageSession = _unitOfWorkContext.Find<SynchronizedStorageSession>();
            var sqlStorageSession = synchronizedStorageSession.GetSqlStorageSession();
            
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseLoggerFactory(_loggerFactory)
                .UseSqlServer(sqlStorageSession.Connection)
                .ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));
            
            var dbContext = new ProviderRelationshipsDbContext(optionsBuilder.Options);
            
            dbContext.Database.UseTransaction(sqlStorageSession.Transaction);

            return dbContext;
        }
    }
}