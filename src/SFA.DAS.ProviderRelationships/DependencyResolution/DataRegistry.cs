using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Azure.Documents;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            For<DbConnection>().Use(c => new SqlConnection(c.GetInstance<ProviderRelationshipsConfiguration>().DatabaseConnectionString));
            For<IDocumentClient>().Use(c => c.GetInstance<IDocumentClientFactory>().CreateDocumentClient()).Singleton();
            For<IDocumentClientFactory>().Use<DocumentClientFactory>();
            For<IProviderRelationshipsDbContextFactory>().Use<ProviderRelationshipsDbContextFactory>();
            For<IRelationshipsRepository>().Use<RelationshipsRepository>();
            For<ProviderRelationshipsDbContext>().Use(c => c.GetInstance<IProviderRelationshipsDbContextFactory>().CreateDbContext());
            For<ITableStorageConfigurationService>().Use<TableStorageConfigurationService>();
            For<IEnvironmentService>().Use<EnvironmentService>();
            For<IAzureTableStorageConnectionAdapter>().Use<AzureTableStorageConnectionAdapter>();
        }
    }
}