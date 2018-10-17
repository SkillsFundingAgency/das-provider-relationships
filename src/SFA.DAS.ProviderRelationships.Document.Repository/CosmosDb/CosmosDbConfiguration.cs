namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public class CosmosDbConfiguration : IDocumentConfiguration
    {
        public string Uri { get; set; }
        public string SecurityKey { get; set; }
        public string DatabaseName { get; set; }
    }
}
