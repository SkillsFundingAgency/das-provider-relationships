using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.Types.ReadStore.Data
{
    public interface IDocumentClientFactory
    {
        IDocumentClient CreateDocumentClient();
    }
}