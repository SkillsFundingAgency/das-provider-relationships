using System;
using System.Net;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Urls
{
    public class EmployerUrls : IEmployerUrls
    {
        private readonly ProviderRelationshipsConfiguration _configuration;

        public EmployerUrls(ProviderRelationshipsConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        #region Accounts
        
        public string YourAccounts() => Accounts("service/accounts");
        public string NotificationSettings() => Accounts("settings/notifications");
        public string RenameAccount(string hashedAccountId = null) => Accounts("rename", hashedAccountId);
        public string YourTeam(string hashedAccountId = null) => Accounts("teams/view", hashedAccountId);
        public string YourOrganisationsAndAgreements(string hashedAccountId = null) => Accounts("agreements", hashedAccountId);
        public string PayeSchemes(string hashedAccountId = null) => Accounts("schemes", hashedAccountId);

        private string Accounts(string path) => Action(_configuration.EmployerAccountsBaseUrl, path);
        private string Accounts(string path, string hashedAccountId) => AccountAction(hashedAccountId, _configuration.EmployerAccountsBaseUrl, path);
        
        #endregion Accounts
        
        #region Commitments

        public string Apprentices(string hashedAccountId) => Commitments("apprentices/home", hashedAccountId);
        
        private string Commitments(string path, string hashedAccountId) => AccountAction(hashedAccountId, _configuration.EmployerCommitmentsBaseUrl, path);

        #endregion Commitments
        
        #region Finance
        
        private string Finance(string path, string hashedAccountId) => AccountAction(hashedAccountId, _configuration.EmployerFinanceBaseUrl, path);
        
        #endregion Finance

        #region Portal

        public string Homepage() => Portal(null);
        public string AccountHomepage(string hashedAccountId = null) => Portal("teams", hashedAccountId);
        public string FinanceHomepage(string hashedAccountId = null) => Portal("finance", hashedAccountId);
        public string SignIn() => Portal("service/signin");
        public string SignOut() => Portal("service/signout");
        public string Help() => Portal("service/help");
        public string Privacy() => Portal("service/privacy");
        
        private string Portal(string path) => Action(_configuration.EmployerPortalBaseUrl, path);
        private string Portal(string path, string hashedAccountId) => AccountAction(hashedAccountId, _configuration.EmployerPortalBaseUrl, path);

        #endregion Portal
        
        #region Recruit
        
        private string Recruit(string path, string hashedAccountId) => AccountAction(hashedAccountId, _configuration.EmployerRecruitBaseUrl, path);
        
        #endregion Recruit
        
        #region Users
        
        public string ChangeEmail() => $"{_authenticationUrls.ChangeEmailUrl}{WebUtility.UrlEncode($"{_configuration.EmployerPortalBaseUrl.TrimEnd('/')}/service/email/change")}";
        public string ChangePassword() => $"{_authenticationUrls.ChangePasswordUrl}{WebUtility.UrlEncode($"{_configuration.EmployerPortalBaseUrl.TrimEnd('/')}/service/password/change")}";
        
        private string Portal(string path) => Action(_configuration.EmployerUsers, path);
        
        #endregion User
        
        private string AccountAction(string accountHashedId, string baseUrl, string path)
        {
            if (string.IsNullOrEmpty(accountHashedId))
            {
                throw new ArgumentException("An account url requires an accountHashedId", nameof(accountHashedId));
            }
            
            return Action(baseUrl, $"accounts/{accountHashedId}/{path}");
        }

        private string Action(string baseUrl, string path)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException("Missing required baseUrl (are all XxBaseUrls populated in ProviderRelationshipsConfiguration?)", nameof(baseUrl));
            }
            
            return $"{baseUrl.TrimEnd('/')}/{path}".TrimEnd('/');
        }
    }
}