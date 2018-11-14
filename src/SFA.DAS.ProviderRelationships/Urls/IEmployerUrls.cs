
namespace SFA.DAS.ProviderRelationships.Urls
{
    public interface IEmployerUrls
    {
        #region Accounts

        string YourAccounts();
        string NotificationSettings();
        string RenameAccount(string hashedAccountId);
        string YourTeam(string hashedAccountId);
        string YourOrganisationsAndAgreements(string hashedAccountId);
        string PayeSchemes(string hashedAccountId);

        #endregion Accounts

        #region Commitments

        string Apprentices(string hashedAccountId);

        #endregion Commitments

        #region Finance
        #endregion Finance

        #region Portal

        string Homepage();
        string AccountHomepage(string hashedAccountId);
        string FinanceHomepage(string hashedAccountId);
        string SignIn();
        string SignOut();
        string Help();
        string Privacy();

        #endregion Portal

        #region Recruit
        #endregion Recruit
    }
}