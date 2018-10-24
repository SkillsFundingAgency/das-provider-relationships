using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public class CosmosClientFactory : IDocumentClientFactory
    {
        public IDocumentClient Create(IDocumentConfiguration cosmosDbConfiguration)
        {
            var connectionPolicy = new ConnectionPolicy();
            connectionPolicy.RetryOptions.MaxRetryAttemptsOnThrottledRequests = cosmosDbConfiguration.MaxRetryAttemptsOnThrottledRequests;
            connectionPolicy.RetryOptions.MaxRetryWaitTimeInSeconds = cosmosDbConfiguration.MaxRetryWaitTimeInSeconds;

            return new DocumentClient(new Uri(cosmosDbConfiguration.Uri), cosmosDbConfiguration.SecurityKey, connectionPolicy);
        }
    }
}