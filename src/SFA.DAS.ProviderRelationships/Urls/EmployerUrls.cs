using System;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Urls
{
    public class EmployerUrls : IEmployerUrls
    {
        private readonly ProviderRelationshipsConfiguration _config;
        
        public EmployerUrls(ProviderRelationshipsConfiguration config)
        {
            _config = config;
        }
        
        #region Accounts
        
        public string YourAccounts() => Accounts("service/accounts");
        public string NotificationSettings() => Accounts("settings/notifications");
        public string RenameAccount(string hashedAccountId) => Accounts("rename", hashedAccountId);
        public string YourTeam(string hashedAccountId) => Accounts("teams/view", hashedAccountId);
        public string YourOrganisationsAndAgreements(string hashedAccountId) => Accounts("agreements", hashedAccountId);
        public string PayeSchemes(string hashedAccountId) => Accounts("schemes", hashedAccountId);

        private string Accounts(string path) => Action(_config.EmployerAccountsBaseUrl, path);
        private string Accounts(string path, string hashedAccountId) => AccountAction(hashedAccountId, _config.EmployerAccountsBaseUrl, path);
        
        #endregion Accounts
        
        #region Commitments

        public string Apprentices(string hashedAccountId) => Commitments("apprentices/home", hashedAccountId);
        
        private string Commitments(string path, string hashedAccountId) => AccountAction(hashedAccountId, _config.EmployerCommitmentsBaseUrl, path);

        #endregion Commitments
        
        #region Finance

        //once finance has been split out...
        //public string FinanceHomepage(string hashedAccountId = null) => Finance("", hashedAccountId);
        
        private string Finance(string path, string hashedAccountId) => AccountAction(hashedAccountId, _config.EmployerFinanceBaseUrl, path);
        
        #endregion Finance

        #region Portal

        public string Homepage() => Portal(null);
        public string AccountHomepage(string hashedAccountId) => Portal("teams", hashedAccountId);
        public string FinanceHomepage(string hashedAccountId) => Portal("finance", hashedAccountId);
        public string SignIn() => Portal("service/signin");
        public string SignOut() => Portal("service/signout");
        public string Help() => Portal("service/help");
        public string Privacy() => Portal("service/privacy");
        
        private string Portal(string path) => Action(_config.EmployerPortalBaseUrl, path);
        private string Portal(string path, string hashedAccountId) => AccountAction(hashedAccountId, _config.EmployerPortalBaseUrl, path);

        #endregion Portal
        
        #region Recruit
        
        private string Recruit(string path, string hashedAccountId) => AccountAction(hashedAccountId, _config.EmployerRecruitBaseUrl, path);
        
        #endregion Recruit
        
        private string AccountAction(string accountHashedId, string baseUrl, string path)
        {
            if (string.IsNullOrEmpty(accountHashedId))
                throw new ArgumentException("An account url requires an accountHashedId", nameof(accountHashedId));
            
            return Action(baseUrl, $"accounts/{accountHashedId}/{path}");
        }

        private string Action(string baseUrl, string path)
        {
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentException("Missing required baseUrl (are all XxBaseUrls populated in ProviderRelationshipsConfiguration?)", nameof(baseUrl));
            
            return $"{baseUrl.TrimEnd('/')}/{path}".TrimEnd('/');
        }
    }
}