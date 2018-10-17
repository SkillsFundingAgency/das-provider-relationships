using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NServiceBus.Persistence;
using SFA.DAS.NServiceBus.ClientOutbox;
using SFA.DAS.NServiceBus.SqlServer.ClientOutbox;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.UnitOfWork;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            For<DbConnection>().Use(c => new SqlConnection(c.GetInstance<ProviderRelationshipsConfiguration>().DatabaseConnectionString));
            For<ProviderRelationshipsDbContext>().Use(c => GetDbContext(c));
        }

        private ProviderRelationshipsDbContext GetDbContext(IContext context)
        {
            var unitOfWorkContext = context.GetInstance<IUnitOfWorkContext>();
            var clientSession = unitOfWorkContext.TryGet<IClientOutboxTransaction>();
            var serverSession = unitOfWorkContext.TryGet<SynchronizedStorageSession>();
            var sqlSession = clientSession?.GetSqlSession() ?? serverSession.GetSqlSession();
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseSqlServer(sqlSession.Connection);
            var db = new ProviderRelationshipsDbContext(optionsBuilder.Options);
            
            db.Database.UseTransaction(sqlSession.Transaction);

            return db;
        }
    }
}