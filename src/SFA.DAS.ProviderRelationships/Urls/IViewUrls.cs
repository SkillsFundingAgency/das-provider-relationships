namespace SFA.DAS.ProviderRelationships.Urls
{
    public interface IViewUrls
    {
        string AccountHashedId { get; set; }

        #region Accounts

        string YourAccounts { get; }

        string NotificationSettings { get; }
        string RenameAccount { get; }
        string YourTeam { get; }
        string YourOrganisationsAndAgreements { get; }
        string PayeSchemes { get; }

        string ChangeEmail { get; }
        string ChangePassword { get; }

        #endregion Accounts

        #region Commitments

        string Apprentices { get; }

        #endregion Commitments

        #region Finance
        #endregion Finance

        #region Portal

        string Homepage { get; }
        string AccountHomepage { get; }
        string FinanceHomepage { get; }
        string SignIn { get; }
        string SignOut { get; }
        string Help { get; }
        string Privacy { get; }

        #endregion Portal

        #region Recruit
        #endregion Recruit
    }
}