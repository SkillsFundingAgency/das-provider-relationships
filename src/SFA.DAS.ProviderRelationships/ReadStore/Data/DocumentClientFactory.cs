using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    public class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly ReadStoreConfiguration _configuration;

        public DocumentClientFactory(ReadStoreConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDocumentClient CreateDocumentClient()
        {
            var connectionPolicy = new ConnectionPolicy
            {
                RetryOptions =
                {
                    //todo: we could use different values for web interactive updates vs off-line batch updates
                    //todo: store these (and api client) values in config?
                    MaxRetryAttemptsOnThrottledRequests = 4,
                    MaxRetryWaitTimeInSeconds = 10
                }
            };

            return new DocumentClient(new Uri(_configuration.Uri), _configuration.AuthKey, connectionPolicy);
        }
    }
}