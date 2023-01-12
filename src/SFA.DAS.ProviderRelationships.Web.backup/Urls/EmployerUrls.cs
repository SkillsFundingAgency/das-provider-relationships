using System;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Web.Urls
{
    public class EmployerUrls : IEmployerUrls
    {
        private readonly IEmployerUrlsConfiguration _employerUrlsConfiguration;
        private string _accountHashedId;

        public EmployerUrls(IEmployerUrlsConfiguration employerUrlsConfiguration)
        {
            _employerUrlsConfiguration = employerUrlsConfiguration;
        }

        public void Initialize(string accountHashedId)
        {
            _accountHashedId = accountHashedId;
        }
        
        #region Accounts
        
        public string Account(string accountHashedId = null) => Accounts("teams", accountHashedId);

        private string Accounts(string path, string accountHashedId) => AccountAction(_employerUrlsConfiguration.EmployerAccountsBaseUrl, path, accountHashedId);
        
        #endregion Accounts

        #region Portal

        public string Homepage() => Portal(null);

        private string Portal(string path) => Action(_employerUrlsConfiguration.EmployerPortalBaseUrl, path);

        #endregion Portal
        
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