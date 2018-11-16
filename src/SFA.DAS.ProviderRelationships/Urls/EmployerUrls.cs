using System;
using System.Net;
using SFA.DAS.ProviderRelationships.Authentication;

namespace SFA.DAS.ProviderRelationships.Urls
{
    public class EmployerUrls : IEmployerUrls
    {
        private readonly IEmployerUrlsConfiguration _employerUrlsConfiguration;
        private readonly IIdentityServerConfiguration _identityServerConfiguration;
        private string _accountHashedId;

        public EmployerUrls(IEmployerUrlsConfiguration employerUrlsConfiguration, IIdentityServerConfiguration identityServerConfiguration)
        {
            _employerUrlsConfiguration = employerUrlsConfiguration;
            _identityServerConfiguration = identityServerConfiguration;
        }

        public void Initialize(string accountHashedId)
        {
            _accountHashedId = accountHashedId;
        }
        
        #region Accounts
        
        public string Account(string hashedAccountId = null) => Accounts("teams", hashedAccountId);
        public string NotificationSettings() => Accounts("settings/notifications");
        public string PayeSchemes(string hashedAccountId = null) => Accounts("schemes", hashedAccountId);
        public string RenameAccount(string hashedAccountId = null) => Accounts("rename", hashedAccountId);
        public string YourAccounts() => Accounts("service/accounts");
        public string YourOrganisationsAndAgreements(string hashedAccountId = null) => Accounts("agreements", hashedAccountId);
        public string YourTeam(string hashedAccountId = null) => Accounts("teams/view", hashedAccountId);

        private string Accounts(string path) => Action(_employerUrlsConfiguration.EmployerAccountsBaseUrl, path);
        private string Accounts(string path, string hashedAccountId) => AccountAction(hashedAccountId, _employerUrlsConfiguration.EmployerAccountsBaseUrl, path);
        
        #endregion Accounts
        
        #region Commitments

        public string Apprentices(string hashedAccountId = null) => Commitments("apprentices/home", hashedAccountId);
        
        private string Commitments(string path, string hashedAccountId) => AccountAction(hashedAccountId, _employerUrlsConfiguration.EmployerCommitmentsBaseUrl, path);

        #endregion Commitments
        
        #region Finance

        public string Finance(string hashedAccountId = null) => Finance("finance", hashedAccountId);
        
        private string Finance(string path, string hashedAccountId) => AccountAction(hashedAccountId, _employerUrlsConfiguration.EmployerFinanceBaseUrl, path);
        
        #endregion Finance

        #region Portal

        public string Help() => Portal("service/help");
        public string Homepage() => Portal(null);
        public string Privacy() => Portal("service/privacy");
        public string SignIn() => Portal("service/signin");
        public string SignOut() => Portal("service/signout");

        private string Portal(string path) => Action(_employerUrlsConfiguration.EmployerPortalBaseUrl, path);
        private string Portal(string path, string hashedAccountId) => AccountAction(hashedAccountId, _employerUrlsConfiguration.EmployerPortalBaseUrl, path);

        #endregion Portal
        
        #region Recruit
        
        public string Recruit(string hashedAccountId = null) => Recruit(null, hashedAccountId);
        
        private string Recruit(string path, string hashedAccountId) => AccountAction(hashedAccountId, _employerUrlsConfiguration.EmployerRecruitBaseUrl, path);
        
        #endregion Recruit
        
        #region Users
        
        public string ChangeEmail() => Users("account/changeemail", Portal("service/email/change"));
        public string ChangePassword() => Users("account/changepassword", Portal("service/password/change"));

        private string Users(string path, string returnPath) => Action(_employerUrlsConfiguration.EmployerUsersBaseUrl, $"{path}?clientId={_identityServerConfiguration.ClientId}&returnurl={WebUtility.UrlEncode(returnPath)}");
        
        #endregion Users
        
        private string AccountAction(string accountHashedId, string baseUrl, string path)
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