namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface ICosmosDbConfiguration
    {
        string Uri { get; set; }
        string SecurityKey { get; set; }
        short MaxRetryAttemptsOnThrottledRequests { get; set; }
        short MaxRetryWaitTimeInSeconds { get; set; }
    }
}
