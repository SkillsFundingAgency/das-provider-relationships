namespace SFA.DAS.ProviderRelationships.Web.Urls
{
    public interface IEmployerUrls
    {
        void Initialize(string accountHashedId);
        
        string Account(string hashedAccountId = null);

        string Homepage();
    }
}