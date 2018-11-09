using System.Web.Mvc;
//using System.Web.UI.WebControls;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Urls;
using SFA.DAS.ProviderRelationships.Web.Routing;

namespace SFA.DAS.ProviderRelationships.Web.Urls
{
    //todo: tests
    public class ApprenticeshipUrls : IApprenticeshipUrls
    {
        private readonly ProviderRelationshipsConfiguration _config;
        
        public ApprenticeshipUrls(ProviderRelationshipsConfiguration config)
        {
            _config = config;
        }

        //public UrlHelper UrlHelper { get; set; }
        public string AccountHashedId { get; set; }
        
        public string EmployerAccountsAction(string path = null)
        {
            return Action(_config.EmployerAccountsBaseUrl, path);
        }

        public string EmployerAccountsAccountAction(string path = null, string hashedAccountId = null)
        {
            return AccountAction(hashedAccountId, _config.EmployerAccountsBaseUrl, path);
        }

        public string EmployerCommitmentsAccountAction(string path = null, string hashedAccountId = null)
        {
            return AccountAction(hashedAccountId, _config.EmployerCommitmentsBaseUrl, path);
        }

        public string EmployerFinanceAccountAction(string path = null, string hashedAccountId = null)
        {
            return AccountAction(hashedAccountId, _config.EmployerFinanceBaseUrl, path);
        }

        public string EmployerPortalAccountAction(string path = null, string hashedAccountId = null)
        {
            return AccountAction(hashedAccountId, _config.EmployerPortalBaseUrl, path);
        }

        public string EmployerPortalAction(string path = null)
        {
            return Action(_config.EmployerPortalBaseUrl, path);
        }

        public string EmployerRecruitAccountAction(string path = null, string hashedAccountId = null)
        {
            return AccountAction(hashedAccountId, _config.EmployerRecruitBaseUrl, path);
        }
        
        private string AccountAction(string accountHashedId, string baseUrl, string path)
        {
//            if (helper == null)
//                helper = UrlHelper;

            if (accountHashedId == null)
                accountHashedId = AccountHashedId;
            
            //var accountHashedId = helper.RequestContext.RouteData.Values[RouteDataKeys.AccountHashedId];
            //todo: if we need the accountHashedId, then won't excluding it create an incorrect url?
            var accountPath = accountHashedId == null ? $"accounts/{path}" : $"accounts/{accountHashedId}/{path}";

            return Action(baseUrl, accountPath);
        }

        public string Action(string baseUrl, string path)
        {
            return $"{baseUrl.TrimEnd('/')}/{path}".TrimEnd('/');
        }
    }
}