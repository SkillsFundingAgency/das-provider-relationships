namespace SFA.DAS.ProviderRelationships.Web.Urls
{
    public interface IEmployerUrls
    {
        void Initialize(string accountHashedId);
        
        #region Accounts
        string Account(string hashedAccountId = null);
        #endregion Accounts

        #region Portal
        string Homepage();
        #endregion Portal
    }
}