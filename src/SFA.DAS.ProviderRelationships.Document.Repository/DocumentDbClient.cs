using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class DocumentdbClient : IDocumentDbClient
    {
        public DocumentdbClient(IDocumentClient documentClient, string databaseName)
        {
            DocumentClient = documentClient;
            DatabaseName = databaseName;
        }
        public string DatabaseName { get; }
        public IDocumentClient DocumentClient { get; }
    }
}
