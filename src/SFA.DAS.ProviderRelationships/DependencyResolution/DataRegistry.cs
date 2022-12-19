using System;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Azure.Services.AppAuthentication;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class DataRegistry : Registry
    {
        private const string AzureResource = "https://database.windows.net/";
        public DataRegistry()
        {
            For<DbConnection>().Use($"Build DbConnection", c => {
                var config = c.GetInstance<ProviderRelationshipsConfiguration>();
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                return config.EnvironmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase)
                    ? new SqlConnection(GetConnectionString(c))
                    : new SqlConnection {
                        ConnectionString = GetConnectionString(c),
                        AccessToken = azureServiceTokenProvider.GetAccessTokenAsync(AzureResource).Result
                    };
            });

            For<ProviderRelationshipsDbContext>().Use(c => c.GetInstance<IProviderRelationshipsDbContextFactory>().CreateDbContext());
        }

        private string GetConnectionString(IContext context)
        {
            return context.GetInstance<ProviderRelationshipsConfiguration>().DatabaseConnectionString;            
        }
    }
}