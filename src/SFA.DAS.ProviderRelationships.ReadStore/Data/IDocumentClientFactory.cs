using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal interface IDocumentClientFactory
    {
        IDocumentClient CreateDocumentClient();
    }
}