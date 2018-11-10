namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Fakes
{
    public class DummyRepository : DocumentRepository<Dummy>
    {
        public DummyRepository(IDocumentDbClient documentDbClient, string collectionName)
            : base(documentDbClient, collectionName)
        {
        }
    }
}