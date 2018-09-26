using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class UrlHelperExtensions
    {
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

        private static string AccountAction(UrlHelper helper, string baseUrl, string path)
        {
            var hashedAccountId = helper.RequestContext.RouteData.Values[UrlParameterKeys.HashedAccountId];
            var accountPath = hashedAccountId == null ? $"accounts/{path}" : $"accounts/{hashedAccountId}/{path}";

            return Action(baseUrl, accountPath);
        }

        private static string Action(string baseUrl, string path)
        {
            var url = $"{baseUrl.TrimEnd('/')}/{path}".TrimEnd('/');

            return url;
        }
    }
}