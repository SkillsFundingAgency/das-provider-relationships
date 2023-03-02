using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NServiceBus.Persistence;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.NServiceBus.SqlServer.Data;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRelationships.Data
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
                .UseSqlServer(sqlStorageSession.Connection);

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