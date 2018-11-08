using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Routing;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public interface IApprenticeshipUrls
    {
        string EmployerAccountsAction(string path = null);

        string EmployerAccountsAccountAction(UrlHelper urlHelper, string path = null);

        string EmployerCommitmentsAccountAction(UrlHelper urlHelper, string path = null);

        string EmployerFinanceAccountAction(UrlHelper urlHelper, string path = null);

        string EmployerPortalAccountAction(UrlHelper urlHelper, string path = null);

        string EmployerPortalAction(string path = null);

        string EmployerRecruitAccountAction(UrlHelper urlHelper, string path = null);
    }

    public class ApprenticeshipUrls
    {
        private readonly ProviderRelationshipsConfiguration _config;
        
        public ApprenticeshipUrls(ProviderRelationshipsConfiguration config)
        {
            _config = config;
        }
        
        public string EmployerAccountsAction(string path = null)
        {
            return Action(_config.EmployerAccountsBaseUrl, path);
        }

        public string EmployerAccountsAccountAction(UrlHelper urlHelper, string path = null)
        {
            return AccountAction(urlHelper, _config.EmployerAccountsBaseUrl, path);
        }

        public string EmployerCommitmentsAccountAction(UrlHelper urlHelper, string path = null)
        {
            return AccountAction(urlHelper, _config.EmployerCommitmentsBaseUrl, path);
        }

        public string EmployerFinanceAccountAction(UrlHelper urlHelper, string path = null)
        {
            return AccountAction(urlHelper, _config.EmployerFinanceBaseUrl, path);
        }

        public string EmployerPortalAccountAction(UrlHelper urlHelper, string path = null)
        {
            return AccountAction(urlHelper, _config.EmployerPortalBaseUrl, path);
        }

        public string EmployerPortalAction(string path = null)
        {
            return Action(_config.EmployerPortalBaseUrl, path);
        }

        public string EmployerRecruitAccountAction(UrlHelper urlHelper, string path = null)
        {
            return AccountAction(urlHelper, _config.EmployerRecruitBaseUrl, path);
        }
        
        private static string AccountAction(UrlHelper helper, string baseUrl, string path)
        {
            var accountHashedId = helper.RequestContext.RouteData.Values[RouteDataKeys.AccountHashedId];
            var accountPath = accountHashedId == null ? $"accounts/{path}" : $"accounts/{accountHashedId}/{path}";

            return Action(baseUrl, accountPath);
        }

        private static string Action(string baseUrl, string path)
        {
            return $"{baseUrl.TrimEnd('/')}/{path}".TrimEnd('/');
        }
    }
}