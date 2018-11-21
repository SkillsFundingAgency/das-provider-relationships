using System;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Azure.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NServiceBus.Persistence;
using SFA.DAS.NServiceBus.ClientOutbox;
using SFA.DAS.NServiceBus.SqlServer.ClientOutbox;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.UnitOfWork;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            For<DbConnection>().Use(c => new SqlConnection(c.GetInstance<ProviderRelationshipsConfiguration>().DatabaseConnectionString));
            For<IDocumentClient>().Use(c =>  c.GetInstance<IDocumentClientFactory>().CreateDocumentClient().GetAwaiter().GetResult()).Singleton();
            For<IDocumentClientFactory>().Use<DocumentClientFactory>();
            For<IRelationshipsRepository>().Use<RelationshipsRepository>();
            For<ProviderRelationshipsDbContext>().Use(c => GetDbContext(c));
            For<ITableStorageConfigurationService>().Use<TableStorageConfigurationService>();
            For<IEnvironmentService>().Use<EnvironmentService>();
        }

        private ProviderRelationshipsDbContext GetDbContext(IContext context)
        {
            var loggerFactory = new LoggerFactory().AddNLog();
            var unitOfWorkContext = context.GetInstance<IUnitOfWorkContext>();
            var clientSession = unitOfWorkContext.Find<IClientOutboxTransaction>();
            var serverSession = unitOfWorkContext.Find<SynchronizedStorageSession>();
            var sqlSession = clientSession?.GetSqlSession() ?? serverSession?.GetSqlSession() ?? throw new Exception("Cannot find the SQL session");
            
            var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                .UseLoggerFactory(loggerFactory)
                .UseSqlServer(sqlSession.Connection);
            
            var db = new ProviderRelationshipsDbContext(optionsBuilder.Options);
            
            db.Database.UseTransaction(sqlSession.Transaction);
            
            return db;
        }
    }
}