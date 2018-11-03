namespace SFA.DAS.ProviderRelationships.Document.Repository.UnitTests
{
    public class CosmosDbConfigurationStub : ICosmosDbConfiguration
    {
        public string Uri { get; set; }
        public string SecurityKey { get; set; }
        public string DatabaseName { get; set; }
        public short MaxRetryAttemptsOnThrottledRequests { get; set; }
        public short MaxRetryWaitTimeInSeconds { get; set; }
    }
}