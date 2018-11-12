using SFA.DAS.ProviderRelationships.Authentication;

namespace SFA.DAS.ProviderRelationships.Urls
{
    public class ViewUrls : IViewUrls
    {
        private readonly IEmployerUrls _employerUrls;
        //private readonly string _accountHashedId;
        private readonly AccountUrls _accountUrls;
        
        //public ViewUrls(IEmployerUrls employerUrls, string accountHashedId, AccountUrls accountUrls)
        public ViewUrls(IEmployerUrls employerUrls, AccountUrls accountUrls)
        {
            _employerUrls = employerUrls;
            //_accountHashedId = accountHashedId;
            _accountUrls = accountUrls;
        }
        
        public string AccountHashedId { get; set; }
        
        #region Accounts

        public string YourAccounts => _employerUrls.YourAccounts();
        public string NotificationSettings => _employerUrls.NotificationSettings();
        public string RenameAccount => _employerUrls.RenameAccount(AccountHashedId);
        public string YourTeam => _employerUrls.YourTeam(AccountHashedId);
        public string YourOrganisationsAndAgreements => _employerUrls.YourOrganisationsAndAgreements(AccountHashedId);
        public string PayeSchemes => _employerUrls.PayeSchemes(AccountHashedId);

        public string ChangeEmail => _accountUrls.ChangeEmailUrl;
        public string ChangePassword => _accountUrls.ChangePasswordUrl;

        #endregion Accounts

        #region Commitments

        public string Apprentices => _employerUrls.Apprentices(AccountHashedId);

        #endregion Commitments

        #region Finance
        #endregion Finance

        #region Portal

        public string Homepage => _employerUrls.Homepage();
        public string AccountHomepage => _employerUrls.AccountHomepage(AccountHashedId);
        public string FinanceHomepage => _employerUrls.FinanceHomepage(AccountHashedId);
        public string SignIn => _employerUrls.SignIn();
        public string SignOut => _employerUrls.SignOut();
        public string Help => _employerUrls.Help();
        public string Privacy => _employerUrls.Privacy();

        #endregion Portal

        #region Recruit
        #endregion Recruit
    }
}