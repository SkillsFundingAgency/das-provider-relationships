using System.Text;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Helpers
{ 
    public class ProviderUrls : IProviderUrls
    {
        private readonly string _providerPortalBaseUrl;
        private const string RecruitSubdomain = "recruit";

        public ProviderUrls(ProviderRelationshipsConfiguration providerRelationshipsConfiguration)
        {
            _providerPortalBaseUrl = providerRelationshipsConfiguration.ProviderPortalBaseUrl;
        }

        public string ProviderManageRecruitEmails(string ukprn) => Recruit(ukprn, "notifications-manage/");
        public string Recruit(string ukprn) => Recruit(ukprn, null);
        private string Recruit(string ukprn, string path) => FormatUrl(_providerPortalBaseUrl, RecruitSubdomain, $"{ukprn}/{path}");

        private static string FormatUrl(string baseUrl, string subdomain, string path)
        {
            var urlString = new StringBuilder();

            urlString.Append(FormatBaseUrl(baseUrl, subdomain));

            if (!string.IsNullOrEmpty(path))
            {
                urlString.Append($"/{path}");
            }

            return urlString.ToString().TrimEnd('/');
        }

        private static string FormatBaseUrl(string url, string subDomain = "", string folder = "")
        {
            var returnUrl = url.EndsWith("/")
                ? url
                : url + "/";

            if (!string.IsNullOrEmpty(subDomain))
            {
                returnUrl = returnUrl.Replace("https://", $"https://{subDomain}.");
            }

            if (!string.IsNullOrEmpty(folder))
            {
                returnUrl = $"{returnUrl}{folder}/";
            }

            return returnUrl;
        }
    }
}