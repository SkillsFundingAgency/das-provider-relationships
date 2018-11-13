using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Fakes
{
    public class DummyRepository : DocumentRepository<Dummy>
    {
        public DummyRepository(IDocumentClient documentClient, string databaseName, string collectionName)
            : base(documentClient, databaseName, collectionName)
        {
        }
    }
}