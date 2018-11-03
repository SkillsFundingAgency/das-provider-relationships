namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Stubs
{
    public class CosmosDbConfigurationStub : ICosmosDbConfiguration
    {
        public string Uri { get; set; }
        public string SecurityKey { get; set; }
    }
}