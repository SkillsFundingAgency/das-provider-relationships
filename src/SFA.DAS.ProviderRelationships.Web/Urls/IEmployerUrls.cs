namespace SFA.DAS.ProviderRelationships.Web.Urls
{
    public interface IEmployerUrls
    {
        void Initialize(string accountHashedId);
        
        #region Accounts

        string Account(string hashedAccountId = null);
        string NotificationSettings();
        string PayeSchemes(string hashedAccountId = null);
        string RenameAccount(string hashedAccountId = null);
        string YourAccounts();
        string YourOrganisationsAndAgreements(string hashedAccountId = null);
        string YourTeam(string hashedAccountId = null);

        #endregion Accounts

        #region Commitments

        string Apprentices(string hashedAccountId = null);

        #endregion Commitments

        #region Finance
        
        string Finance(string hashedAccountId = null);
        
        #endregion Finance

        #region Portal

        string Help();
        string Homepage();
        string Privacy();
        string SignIn();
        string SignOut();

        #endregion Portal

        #region Recruit
        
        string Recruit(string hashedAccountId = null);
        
        #endregion Recruit

        #region Users
        
        string ChangeEmail();
        string ChangePassword();

        #endregion Users
    }
}