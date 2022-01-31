using System;
using System.Net;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Web.Urls
{
    public class EmployerUrls : IEmployerUrls
    {
        private readonly IEmployerUrlsConfiguration _employerUrlsConfiguration;
        private readonly IOidcConfiguration _oidcConfiguration;
        private string _accountHashedId;

        public EmployerUrls(IEmployerUrlsConfiguration employerUrlsConfiguration, IOidcConfiguration oidcConfiguration)
        {
            _employerUrlsConfiguration = employerUrlsConfiguration;
            _oidcConfiguration = oidcConfiguration;
        }

        public void Initialize(string accountHashedId)
        {
            _accountHashedId = accountHashedId;
        }
        
        #region Accounts
        
        public string Account(string accountHashedId = null) => Accounts("teams", accountHashedId);
        public string NotificationSettings() => Accounts("settings/notifications");
        public string PayeSchemes(string accountHashedId = null) => Accounts("schemes", accountHashedId);
        public string RenameAccount(string accountHashedId = null) => Accounts("rename", accountHashedId);
        public string YourAccounts() => Accounts("service/accounts");
        public string YourOrganisationsAndAgreements(string accountHashedId = null) => Accounts("agreements", accountHashedId);
        public string YourTeam(string accountHashedId = null) => Accounts("teams/view", accountHashedId);

        private string Accounts(string path) => Action(_employerUrlsConfiguration.EmployerAccountsBaseUrl, path);
        private string Accounts(string path, string accountHashedId) => AccountAction(_employerUrlsConfiguration.EmployerAccountsBaseUrl, path, accountHashedId);
        
        #endregion Accounts
        
        #region Commitments

        public string Apprentices(string accountHashedId = null) => Commitments("apprentices/home", accountHashedId);
        
        private string Commitments(string path, string accountHashedId) => AccountAction(_employerUrlsConfiguration.EmployerCommitmentsBaseUrl, path, accountHashedId);

        #endregion Commitments
        
        #region Finance

        public string Finance(string accountHashedId = null) => Finance("finance", accountHashedId);
        
        private string Finance(string path, string accountHashedId) => AccountAction(_employerUrlsConfiguration.EmployerFinanceBaseUrl, path, accountHashedId);
        
        #endregion Finance

        #region Portal

        public string Help() => Portal("service/help");
        public string Homepage() => Portal(null);
        public string Privacy() => Portal("service/privacy");
        public string SignIn() => Portal("service/signin");
        public string SignOut() => Portal("service/signout");

        private string Portal(string path) => Action(_employerUrlsConfiguration.EmployerPortalBaseUrl, path);
        private string Portal(string path, string accountHashedId) => AccountAction(_employerUrlsConfiguration.EmployerPortalBaseUrl, path, accountHashedId);

        #endregion Portal
        
        #region Recruit
        
        public string Recruit(string accountHashedId = null) => Recruit(null, accountHashedId);
        
        private string Recruit(string path, string accountHashedId) => AccountAction(_employerUrlsConfiguration.EmployerRecruitBaseUrl, path, accountHashedId);
        
        #endregion Recruit
        
        #region Users
        
        public string ChangeEmail() => Users("account/changeemail", Portal("service/email/change"));
        public string ChangePassword() => Users("account/changepassword", Portal("service/password/change"));

        private string Users(string path, string returnUrl) => Action(_employerUrlsConfiguration.EmployerUsersBaseUrl, $"{path}?clientId={_oidcConfiguration.ClientId}&returnurl={WebUtility.UrlEncode(returnUrl)}");
        
        #endregion Users
        
        private string AccountAction(string baseUrl, string path, string accountHashedId)
        {
            if (accountHashedId == null)
            {
                accountHashedId = _accountHashedId;
            }

            if (string.IsNullOrWhiteSpace(accountHashedId))
            {
                throw new ArgumentException($"Value cannot be null or white space", nameof(accountHashedId));
            }
            
            return Action(baseUrl, $"accounts/{accountHashedId}/{path}");
        }

        private string Action(string baseUrl, string path)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentException("Value cannot be null or white space", nameof(baseUrl));
            }
            
            return $"{baseUrl.TrimEnd('/')}/{path}".TrimEnd('/');
        }
    }
}