using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SFA.DAS.AutoConfiguration;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly ITableStorageConfigurationService _configurationService;

        public DocumentClientFactory(ITableStorageConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public IDocumentClient CreateDocumentClient()
        {
            var connectionPolicy = new ConnectionPolicy
            {
                RetryOptions =
                {
                    MaxRetryAttemptsOnThrottledRequests = 3,
                    MaxRetryWaitTimeInSeconds = 2
                }
            };

            var configuration = _configurationService.Get<ReadStoreConfiguration>();

            return new DocumentClient(new Uri(configuration.Uri), configuration.AuthKey, connectionPolicy);
        }
    }
}