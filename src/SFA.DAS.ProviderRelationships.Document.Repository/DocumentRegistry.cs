using StructureMap;

namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class DocumentRegistry : Registry
    {
        public DocumentRegistry()
        {
            For<IDocumentClientFactory>().Use<CosmosClientFactory>();
            For(typeof(IDocumentDbClient<>)).Use(typeof(CosmosDbClient<>));
            For(typeof(IDocumentRepository<>)).Use(typeof(DocumentRepository<>));
            For(typeof(IDocumentReadOnlyRepository<>)).Use(typeof(DocumentReadOnlyRepository<>));
        }
    }

}
