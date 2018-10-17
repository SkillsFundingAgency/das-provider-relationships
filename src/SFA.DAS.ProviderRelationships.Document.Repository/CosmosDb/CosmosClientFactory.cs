using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public class CosmosClientFactory : IDocumentClientFactory
    {
        public IDocumentClient Create(IDocumentConfiguration cosmosDbConfiguration)
        {
            return new DocumentClient(new Uri(cosmosDbConfiguration.Uri), cosmosDbConfiguration.SecurityKey);
        }
    }
}