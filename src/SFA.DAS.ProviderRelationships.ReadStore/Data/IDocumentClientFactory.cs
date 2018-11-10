using SFA.DAS.ProviderRelationships.Document.Repository;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal interface IDocumentClientFactory
    {
        IDocumentDbClient CreateDocumentDbClient();
    }
}