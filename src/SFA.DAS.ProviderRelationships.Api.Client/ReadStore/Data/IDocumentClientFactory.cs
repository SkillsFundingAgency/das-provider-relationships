using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data
{
    public interface IDocumentClientFactory
    {
        IDocumentClient CreateDocumentClient();
    }
}