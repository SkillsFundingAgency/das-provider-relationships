using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Routing;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public class DasUrls
    {
        private readonly ProviderRelationshipsConfiguration _config;
        
        public DasUrls(ProviderRelationshipsConfiguration config)
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
    
    public static class UrlHelperExtensions
    {
        public static string EmployerAccountsAction(this UrlHelper urlHelper, string path = null)
        {
            var configuration = DependencyResolver.Current.GetService<ProviderRelationshipsConfiguration>();

            return Action(configuration.EmployerAccountsBaseUrl, path);
        }

        public static string EmployerAccountsAccountAction(this UrlHelper urlHelper, string path = null)
        {
            var configuration = DependencyResolver.Current.GetService<ProviderRelationshipsConfiguration>();

            return AccountAction(urlHelper, configuration.EmployerAccountsBaseUrl, path);
        }

        public static string EmployerCommitmentsAccountAction(this UrlHelper urlHelper, string path = null)
        {
            var configuration = DependencyResolver.Current.GetService<ProviderRelationshipsConfiguration>();

            return AccountAction(urlHelper, configuration.EmployerCommitmentsBaseUrl, path);
        }

        public static string EmployerFinanceAccountAction(this UrlHelper urlHelper, string path = null)
        {
            var configuration = DependencyResolver.Current.GetService<ProviderRelationshipsConfiguration>();

            return AccountAction(urlHelper, configuration.EmployerFinanceBaseUrl, path);
        }

        public static string EmployerPortalAccountAction(this UrlHelper urlHelper, string path = null)
        {
            var configuration = DependencyResolver.Current.GetService<ProviderRelationshipsConfiguration>();

            return AccountAction(urlHelper, configuration.EmployerPortalBaseUrl, path);
        }

        public static string EmployerPortalAction(this UrlHelper urlHelper, string path = null)
        {
            var configuration = DependencyResolver.Current.GetService<ProviderRelationshipsConfiguration>();

            return Action(configuration.EmployerPortalBaseUrl, path);
        }

        public static string EmployerRecruitAccountAction(this UrlHelper urlHelper, string path = null)
        {
            var configuration = DependencyResolver.Current.GetService<ProviderRelationshipsConfiguration>();

            return AccountAction(urlHelper, configuration.EmployerRecruitBaseUrl, path);
        }

        private static string AccountAction(UrlHelper helper, string baseUrl, string path)
        {
            var accountHashedId = helper.RequestContext.RouteData.Values[RouteDataKeys.AccountHashedId];
            var accountPath = accountHashedId == null ? $"accounts/{path}" : $"accounts/{accountHashedId}/{path}";

            return Action(baseUrl, accountPath);
        }

        private static string Action(string baseUrl, string path)
        {
            var url = $"{baseUrl.TrimEnd('/')}/{path}".TrimEnd('/');

            return url;
        }
    }
}