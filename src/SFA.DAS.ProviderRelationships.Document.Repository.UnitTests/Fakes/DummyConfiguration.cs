namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests.Fakes
{
    public class DummyConfiguration : ICosmosDbConfiguration
    {
        public string Uri { get; set; }
        public string AuthKey { get; set; }
    }
}