using Microsoft.Azure.Documents;

namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Stubs
{
    public class DocumentRepositoryStub : DocumentRepository<DocumentStub>
    {
        public DocumentRepositoryStub(IDocumentClient documentClient, string databaseName, string collectionName)
            : base(documentClient, databaseName, collectionName)
        {
        }
    }
}