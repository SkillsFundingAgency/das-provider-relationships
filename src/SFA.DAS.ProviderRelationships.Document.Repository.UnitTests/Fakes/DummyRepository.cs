namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Fakes
{
    public class DummyRepository : DocumentRepository<Dummy>
    {
        public DummyRepository(IDocumentDbClient documentClient, string collectionName)
            : base(documentClient, collectionName)
        {
        }
    }
}