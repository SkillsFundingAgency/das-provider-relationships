using Microsoft.Azure.Documents;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentClientFactory
    {
        IDocumentClient Create(IDocumentConfiguration documentConfiguration);
    }
}