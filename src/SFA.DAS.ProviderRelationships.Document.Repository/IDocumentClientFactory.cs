using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentClientFactory
    {
        IDocumentClient Create(IDocumentConfiguration documentConfiguration);
    }
}