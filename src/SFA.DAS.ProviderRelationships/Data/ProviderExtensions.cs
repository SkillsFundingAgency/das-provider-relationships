using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SFA.DAS.ProviderRelationships.Data
{
    public static class ProviderExtensions
    {
        public static Task ImportProviders(this DatabaseFacade database, DataTable providersDataTable)
        {
            var providers = new SqlParameter("providers", SqlDbType.Structured)
            {
                TypeName = "ProvidersType",
                Value = providersDataTable
            };
            
            var now = new SqlParameter("now", DateTime.UtcNow);
            
            return database.ExecuteSqlCommandAsync("EXEC ImportProviders @providers, @now", providers, now);
        }
    }
}