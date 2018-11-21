using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly ITableStorageConfigurationService _configurationService;

        public DocumentClientFactory(ITableStorageConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task<IDocumentClient> CreateDocumentClient()
        {
            var connectionPolicy = new ConnectionPolicy
            {
                RetryOptions =
                {
                    MaxRetryAttemptsOnThrottledRequests = 3,
                    MaxRetryWaitTimeInSeconds = 2
                }
            };

            var configuration = await _configurationService.Get<ReadStoreConfiguration>();

            return new DocumentClient(new Uri(configuration.Uri), configuration.AuthKey, connectionPolicy);
        }
    }
}