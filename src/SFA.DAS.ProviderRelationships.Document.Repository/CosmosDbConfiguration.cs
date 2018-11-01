namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public class CosmosDbConfiguration : IDocumentConfiguration
    {
        public CosmosDbConfiguration()
        {
        }

        public CosmosDbConfiguration(string url, string securtyKey, string databaseName)
        {
            Uri = url;
            SecurityKey = securtyKey;
            DatabaseName = databaseName;
        }

        public string Uri { get; set; }
        public string SecurityKey { get; set; }
        public string DatabaseName { get; set; }
        public short MaxRetryAttemptsOnThrottledRequests { get; set; }
        public short MaxRetryWaitTimeInSeconds { get; set; }
    }
}
