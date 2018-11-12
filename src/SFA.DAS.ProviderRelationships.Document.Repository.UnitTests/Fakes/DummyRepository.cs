using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Fakes
{
    public class DummyRepository : DocumentRepository<Dummy>
    {
        public DummyRepository(IDocumentClient documentDbClient, string databaseName, string collectionName)
            : base(documentDbClient, databaseName, collectionName)
        {
        }
    }
}