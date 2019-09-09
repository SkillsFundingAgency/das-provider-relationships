using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data
{
    public class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly Lazy<IDocumentClient> _documentClient;

        public DocumentClientFactory(ReadStoreConfiguration configuration)
        {
            _documentClient = new Lazy<IDocumentClient>(() => new DocumentClient(new Uri(configuration.Uri), configuration.AuthKey, new ConnectionPolicy
            {
                RetryOptions =
                {
                    MaxRetryAttemptsOnThrottledRequests = 2,
                    MaxRetryWaitTimeInSeconds = 2
                }
            }));
        }

        public IDocumentClient CreateDocumentClient()
        {
            return _documentClient.Value; 
        }
    }
}