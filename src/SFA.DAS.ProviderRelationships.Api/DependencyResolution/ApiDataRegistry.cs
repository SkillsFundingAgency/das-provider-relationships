using System.Data.Common;
using System.Data.SqlClient;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.DependencyResolution
{
    public class ApiDataRegistry : Registry
    {
        public ApiDataRegistry()
        {
            For<DbConnection>().Use(c => new SqlConnection(c.GetInstance<ProviderRelationshipsConfiguration>().DatabaseConnectionString));
            For<IProviderRelationshipsDbContextFactory>().Use<ProviderRelationshipsApiDbContextFactory>();
            For<ProviderRelationshipsDbContext>().Use(c => c.GetInstance<IProviderRelationshipsDbContextFactory>().CreateDbContext());
        }
    }
}