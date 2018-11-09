using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Urls
{
    //todo: tests
    public class EmployerUrls : IEmployerUrls
    {
        private readonly ProviderRelationshipsConfiguration _config;
        
        public EmployerUrls(ProviderRelationshipsConfiguration config)
        {
            _config = config;
        }

        public string AccountHashedId { get; set; }
        
        #region Accounts
        
        public string YourAccounts() => Accounts("service/accounts");
        public string NotificationSettings() => Accounts("settings/notifications");
        public string RenameAccount(string hashedAccountId = null) => Accounts("rename", hashedAccountId);
        public string YourTeam(string hashedAccountId = null) => Accounts("teams/view", hashedAccountId);
        public string YourOrganisationsAndAgreements(string hashedAccountId = null) => Accounts("agreements", hashedAccountId);
        public string PayeSchemes(string hashedAccountId = null) => Accounts("schemes", hashedAccountId);

        private string Accounts(string path) => Action(_config.EmployerAccountsBaseUrl, path);
        private string Accounts(string path, string hashedAccountId) => AccountAction(hashedAccountId, _config.EmployerAccountsBaseUrl, path);
        
        #endregion Accounts
        
        #region Commitments

        public string Apprentices(string hashedAccountId = null) => Commitments("apprentices/home", hashedAccountId);
        
        private string Commitments(string path, string hashedAccountId) => AccountAction(hashedAccountId, _config.EmployerCommitmentsBaseUrl, path);

        #endregion Commitments
        
        #region Finance

        //once finance has been split out...
        //public string FinanceHomepage(string hashedAccountId = null) => Finance("", hashedAccountId);
        
        private string Finance(string path, string hashedAccountId) => AccountAction(hashedAccountId, _config.EmployerFinanceBaseUrl, path);
        
        #endregion Finance

        #region Portal

        //todo: some use home with account & some without. separate pages?
        public string PortalHomepage(string hashedAccountId = null) => Portal("teams", hashedAccountId);
        public string FinanceHomepage(string hashedAccountId = null) => Portal("finance", hashedAccountId);
        public string SignIn() => Portal("service/signin");
        public string SignOut() => Portal("service/signout");
        public string Help() => Portal("service/help");
        public string Privacy() => Portal("service/privacy");
        
        private string Portal(string path) => Action(_config.EmployerPortalBaseUrl, path);
        private string Portal(string path, string hashedAccountId) => AccountAction(hashedAccountId, _config.EmployerPortalBaseUrl, path);

        public string EmployerPortalAction(string path = null)
        {
            return Action(_config.EmployerPortalBaseUrl, path);
        }

        #endregion Portal
        
        #region Recruit
        
        private string Recruit(string path, string hashedAccountId) => AccountAction(hashedAccountId, _config.EmployerRecruitBaseUrl, path);
        
        #endregion Recruit
        
        private string AccountAction(string accountHashedId, string baseUrl, string path)
        {
            if (accountHashedId == null)
                accountHashedId = AccountHashedId;
            
            //todo: if we need the accountHashedId, then won't excluding it create an incorrect url?
            var accountPath = accountHashedId == null ? $"accounts/{path}" : $"accounts/{accountHashedId}/{path}";

            return Action(baseUrl, accountPath);
        }

        private string Action(string baseUrl, string path)
        {
            return $"{baseUrl.TrimEnd('/')}/{path}".TrimEnd('/');
        }
    }
}