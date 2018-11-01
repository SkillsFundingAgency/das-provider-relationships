using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class CosmosClientFactory : IDocumentClientFactory
    {
        public IDocumentClient Create(IDocumentConfiguration cosmosDbConfiguration)
        {
            var connectionPolicy = new ConnectionPolicy {
                RetryOptions = {
                    MaxRetryAttemptsOnThrottledRequests = cosmosDbConfiguration.MaxRetryAttemptsOnThrottledRequests,
                    MaxRetryWaitTimeInSeconds = cosmosDbConfiguration.MaxRetryWaitTimeInSeconds
                }
            };

            return new DocumentClient(new Uri(cosmosDbConfiguration.Uri), cosmosDbConfiguration.SecurityKey, connectionPolicy);
        }
    }
}