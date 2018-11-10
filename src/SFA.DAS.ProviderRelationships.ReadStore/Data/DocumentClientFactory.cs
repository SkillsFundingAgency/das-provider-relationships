using System;
using Microsoft.Azure.Documents.Client;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly ProviderRelationshipsReadStoreConfiguration _configuration;

        public DocumentClientFactory(ProviderRelationshipsReadStoreConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDocumentDbClient CreateDocumentDbClient()
        {
            var connectionPolicy = new ConnectionPolicy
            {
                RetryOptions =
                {
                    MaxRetryAttemptsOnThrottledRequests = 3,
                    MaxRetryWaitTimeInSeconds = 2
                }
            };

            var documentClient = new DocumentClient(new Uri(_configuration.Uri), _configuration.AuthKey, connectionPolicy);
            var databaseName = _configuration.DatabaseName ?? "SFA.DAS.ProviderRelationships.ReadStore.Database";

            return new DocumentdbClient(documentClient, databaseName);
        }
    }
}