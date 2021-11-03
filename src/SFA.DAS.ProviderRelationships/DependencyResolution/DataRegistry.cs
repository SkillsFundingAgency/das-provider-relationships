using System;
using System.Configuration;
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
            var environmentName = ConfigurationManager.AppSettings["EnvironmentName"];
            For<DbConnection>().Use($"Build DbConnection", c => {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                return environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase)
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
            var config = context.GetInstance<ProviderRelationshipsConfiguration>();
            return config.DatabaseConnectionString;
        }
    }
}