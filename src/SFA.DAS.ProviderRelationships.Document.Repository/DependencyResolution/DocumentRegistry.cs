using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Document.Repository.DependencyResolution
{
    public class DocumentRegistry : Registry
    {
        public DocumentRegistry()
        {
            For<IDocumentClientFactory>().Use<CosmosClientFactory>();
            For(typeof(IDocumentRepository<>)).Use(typeof(DocumentRepository<>));
            For<IDocumentConfiguration>().Use<CosmosDbConfiguration>();

            //For<CosmosDbConfiguration>().Use(new CosmosDbConfiguration
            //{
            //    DatabaseName = "SFA",
            //    Uri = "https://localhost:8081",
            //    SecurityKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
            //});
        }

    }

}
