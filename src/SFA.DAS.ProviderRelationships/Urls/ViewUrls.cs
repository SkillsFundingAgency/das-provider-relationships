using SFA.DAS.ProviderRelationships.Authentication;

namespace SFA.DAS.ProviderRelationships.Urls
{
    public class ViewUrls
    {
        private readonly IEmployerUrls _employerUrls;
        private readonly string _accountHashedId;
        private readonly AccountUrls _accountUrls;
        
        public ViewUrls(IEmployerUrls employerUrls, string accountHashedId, AccountUrls accountUrls)
        {
            _employerUrls = employerUrls;
            _accountHashedId = accountHashedId;
            _accountUrls = accountUrls;
        }
        
        #region Accounts

        public string YourAccounts => _employerUrls.YourAccounts();
        public string NotificationSettings => _employerUrls.NotificationSettings();
        public string RenameAccount => _employerUrls.RenameAccount(_accountHashedId);
        public string YourTeam => _employerUrls.YourTeam(_accountHashedId);
        public string YourOrganisationsAndAgreements => _employerUrls.YourOrganisationsAndAgreements(_accountHashedId);
        public string PayeSchemes => _employerUrls.PayeSchemes(_accountHashedId);

        public string ChangeEmail => _accountUrls.ChangeEmailUrl;
        public string ChangePassword => _accountUrls.ChangePasswordUrl;

        #endregion Accounts

        #region Commitments

        public string Apprentices => _employerUrls.Apprentices(_accountHashedId);

        #endregion Commitments

        #region Finance
        #endregion Finance

        #region Portal

        public string Homepage => _employerUrls.Homepage();
        public string AccountHomepage => _employerUrls.AccountHomepage(_accountHashedId);
        public string FinanceHomepage => _employerUrls.FinanceHomepage(_accountHashedId);
        public string SignIn => _employerUrls.SignIn();
        public string SignOut => _employerUrls.SignOut();
        public string Help => _employerUrls.Help();
        public string Privacy => _employerUrls.Privacy();

        #endregion Portal

        #region Recruit
        #endregion Recruit
    }
}