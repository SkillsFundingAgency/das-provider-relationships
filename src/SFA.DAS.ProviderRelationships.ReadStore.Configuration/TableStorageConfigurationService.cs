using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Configuration
{
    public class TableStorageConfigurationService : ITableStorageConfigurationService
    {
        private const string ConfigurationTableReference = "Configuration";
        private const string DefaultVersion = "1.0";
        private readonly IEnvironmentService _environmentService;

        public TableStorageConfigurationService(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }

        public async Task<T> Get<T>()
        {
            var environmentName = _environmentService.GetVariable(EnvironmentVariableNames.Environment);
            var storageConnectionString = _environmentService.GetVariable(EnvironmentVariableNames.ConfigurationStorageConnectionString);
            var rowKey = $"{Assembly.GetAssembly(typeof(T)).GetName().Name}_{DefaultVersion}";

            var conn = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = conn.CreateCloudTableClient();
            var table = tableClient.GetTableReference(ConfigurationTableReference);

            var operation = TableOperation.Retrieve(environmentName, rowKey);
            TableResult result;
            try
            {
                result = await table.ExecuteAsync(operation);
            }
            catch (Exception e)
            {
                throw new Exception("Could not connect to Storage to retrieve settings.", e);
            }

            var dynResult = result.Result as DynamicTableEntity;
            var data = dynResult.Properties["Data"].StringValue;

            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
