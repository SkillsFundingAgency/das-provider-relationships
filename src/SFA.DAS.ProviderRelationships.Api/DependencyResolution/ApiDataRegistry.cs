using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Azure.Documents;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.DependencyResolution;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.DependencyResolution
{
    public class ApiDataRegistry : Registry
    {
        public ApiDataRegistry()
        {
            For<DbConnection>().Use(c => new SqlConnection(c.GetInstance<ProviderRelationshipsConfiguration>().DatabaseConnectionString));
            For<IDocumentClient>().Use(c => c.GetInstance<IDocumentClientFactory>().CreateDocumentClient()).Singleton();
            For<IDocumentClientFactory>().Use<DocumentClientFactory>();
            For<IProviderRelationshipsDbContextFactory>().Use<ProviderRelationshipsApiDbContextFactory>();
            For<IAccountProviderLegalEntitiesRepository>().Use<AccountProviderLegalEntitiesRepository>();
            For<ProviderRelationshipsDbContext>().Use(c => c.GetInstance<IProviderRelationshipsDbContextFactory>().CreateDbContext());
        }
    }
}