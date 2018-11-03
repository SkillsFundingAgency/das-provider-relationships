using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly DocumentClient _documentClient;

        public DocumentClientFactory(ProviderRelationshipsReadStoreConfiguration configuration)
        {
            var serviceEndpoint = new Uri(configuration.Uri);
            
            var connectionPolicy = new ConnectionPolicy
            {
                RetryOptions =
                {
                    MaxRetryAttemptsOnThrottledRequests = 3,
                    MaxRetryWaitTimeInSeconds = 2
                }
            };

            _documentClient = new DocumentClient(serviceEndpoint, configuration.SecurityKey, connectionPolicy);
        }

        public IDocumentClient CreateDocumentClient()
        {
            return _documentClient;
        }
    }
}