namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface ICosmosDbConfiguration
    {
        string Uri { get; set; }
        string AuthKey { get; set; }
    }
}