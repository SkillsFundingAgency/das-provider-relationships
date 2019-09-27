using System.Data.Common;
using System.Data.SqlClient;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRelationships.Domain.Data;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.DependencyResolution
{
    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            For<DbConnection>().Use(c => new SqlConnection(c.GetInstance<ProviderRegistrationsSettings>().DatabaseConnectionString));
            For<ProviderRelationshipsDbContext>().Use(c => c.GetInstance<IProviderRelationshipsDbContextFactory>().CreateDbContext());
        }
    }
}