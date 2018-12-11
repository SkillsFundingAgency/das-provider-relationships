using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    public interface IDocumentClientFactory
    {
        IDocumentClient CreateDocumentClient();
    }
}