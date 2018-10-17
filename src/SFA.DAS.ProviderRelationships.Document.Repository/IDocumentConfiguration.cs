namespace SFA.DAS.ProviderRelationships.Document.Repository
{
    public interface IDocumentConfiguration
    {
        string Uri { get; set; }
        string SecurityKey { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
    }
}
