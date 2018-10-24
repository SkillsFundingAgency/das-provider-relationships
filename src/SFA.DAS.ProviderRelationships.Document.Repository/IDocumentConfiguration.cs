namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentConfiguration
    {
        string Uri { get; set; }
        string SecurityKey { get; set; }
        string DatabaseName { get; set; }
        short MaxRetryAttemptsOnThrottledRequests { get; set; }
        short MaxRetryWaitTimeInSeconds { get; set; }
    }
}
