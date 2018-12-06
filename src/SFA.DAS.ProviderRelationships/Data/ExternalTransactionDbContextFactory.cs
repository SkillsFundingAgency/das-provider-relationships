using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using NServiceBus.Persistence;
using SFA.DAS.NServiceBus.ClientOutbox;
using SFA.DAS.NServiceBus.SqlServer.ClientOutbox;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class ExternalTransactionDbContextFactory : IProviderRelationshipsDbContextFactory
    {
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly ILoggerFactory _loggerFactory;

        public ExternalTransactionDbContextFactory(IUnitOfWorkContext unitOfWorkContext, ILoggerFactory loggerFactory)
        {
            _unitOfWorkContext = unitOfWorkContext;
            _loggerFactory = loggerFactory;
        }

        public ProviderRelationshipsDbContext CreateDbContext()
        {
            var clientSession = _unitOfWorkContext.Find<IClientOutboxTransaction>();
            var serverSession = _unitOfWorkContext.Find<SynchronizedStorageSession>();
            var sqlSession = clientSession?.GetSqlSession() ?? serverSession?.GetSqlSession() ?? throw new Exception("Cannot find the SQL session");
            
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseLoggerFactory(_loggerFactory)
                .UseSqlServer(sqlSession.Connection)
                .ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));
            
            var dbContext = new ProviderRelationshipsDbContext(optionsBuilder.Options);
            
            dbContext.Database.UseTransaction(sqlSession.Transaction);

            return dbContext;
        }
    }
}