namespace SFA.DAS.ProviderRelationships.Web.Urls;

public interface IEmployerUrls
{
    void Initialize(string accountHashedId);
        
    string Account(string accountHashedId = null);

    string Homepage();
}