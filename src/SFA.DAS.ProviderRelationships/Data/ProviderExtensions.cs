using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SFA.DAS.ProviderRelationships.Data
{
    public static class ProviderExtensions
    {
        public static Task ImportProviders(this ProviderRelationshipsDbContext db, DataTable providersDataTable)
        {
            var providers = new SqlParameter("providers", SqlDbType.Structured)
            {
                TypeName = "Providers",
                Value = providersDataTable
            };
            
            var now = new SqlParameter("now", DateTime.UtcNow);
            
            return db.ExecuteSqlCommandAsync("EXEC ImportProviders @providers, @now", providers, now);
        }
    }
}