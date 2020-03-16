using System;
using System.Linq;
using System.Web.Mvc;
using SFA.DAS.MA.Shared.UI.Configuration;
using SFA.DAS.MA.Shared.UI.Models;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.App_Start;
using SFA.DAS.ProviderRelationships.Web.Urls;
using System.Web;
using SFA.DAS.ProviderRelationships.Web.RouteValues;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IEmployerUrls EmployerUrls(this HtmlHelper htmlHelper)
        {
            return (IEmployerUrls)htmlHelper.ViewBag.EmployerUrls;
        }

        public static MvcHtmlString SetZenDeskLabels(this HtmlHelper html, params string[] labels)
        {
            var keywords = string.Join(",", labels
                .Where(label => !string.IsNullOrEmpty(label))
                .Select(label => $"'{EscapeApostrophes(label)}'"));

            // when there are no keywords default to empty string to prevent zen desk matching articles from the url
            var apiCallString = "<script type=\"text/javascript\">zE('webWidget', 'helpCenter:setSuggestions', { labels: ["
                + (!string.IsNullOrEmpty(keywords) ? keywords : "''")
                + "] });</script>";

            return MvcHtmlString.Create(apiCallString);
        }

        private static string EscapeApostrophes(string input)
        {
            return input.Replace("'", @"\'");
        }

        public static string GetZenDeskSnippetKey(this HtmlHelper html)
        {
            var configuration = DependencyResolver.Current.GetService<ProviderRelationshipsConfiguration>();
            return configuration.ZenDeskSnippetKey;
        }

        public static string GetZenDeskSnippetSectionId(this HtmlHelper html)
        {
            var configuration = DependencyResolver.Current.GetService<ProviderRelationshipsConfiguration>();            
            return configuration.ZenDeskSectionId;
        }

        public static IHeaderViewModel GetHeaderViewModel(this HtmlHelper html)
        {   
            var configuration = DependencyResolver.Current.GetService<IEmployerUrlsConfiguration>();
            var oidcConfiguration = DependencyResolver.Current.GetService<IOidcConfiguration>();
            var requestRoot = GetRootUrl(html.ViewContext.HttpContext.Request);

            var headerModel = new HeaderViewModel(new HeaderConfiguration {
                ManageApprenticeshipsBaseUrl = configuration.EmployerAccountsBaseUrl,
                ApplicationBaseUrl = configuration.EmployerAccountsBaseUrl,
                EmployerCommitmentsBaseUrl = configuration.EmployerCommitmentsBaseUrl,
                EmployerFinanceBaseUrl = configuration.EmployerFinanceBaseUrl,
                AuthenticationAuthorityUrl = oidcConfiguration.BaseAddress,
                ClientId = oidcConfiguration.ClientId,
                EmployerRecruitBaseUrl = configuration.EmployerRecruitBaseUrl,
                SignOutUrl = new Uri($"{requestRoot}/signOut"),
                ChangeEmailReturnUrl = new System.Uri(configuration.EmployerPortalBaseUrl + "/service/email/change"),
                ChangePasswordReturnUrl = new System.Uri(configuration.EmployerPortalBaseUrl + "/service/password/change")
            },
            new UserContext {
                User = html.ViewContext.HttpContext.User,
                HashedAccountId = html.ViewContext.RouteData.Values[RouteValueKeys.AccountHashedId]?.ToString()
            });

            headerModel.SelectMenu("home");         

            return headerModel;
        }

        public static IFooterViewModel GetFooterViewModel(this HtmlHelper html)
        {
            var configuration = DependencyResolver.Current.GetService<IEmployerUrlsConfiguration>();

            return new FooterViewModel(new FooterConfiguration {
                ManageApprenticeshipsBaseUrl = configuration.EmployerAccountsBaseUrl
            },
            new UserContext {
                User = html.ViewContext.HttpContext.User,
                HashedAccountId = html.ViewContext.RouteData.Values[RouteValueKeys.AccountHashedId]?.ToString()
            }
            );
        }

        private static string GetRootUrl(HttpRequestBase request)
        {
            var requestUrl = new Uri(request.Url.AbsoluteUri);

            return $"{requestUrl.Scheme}://{requestUrl.Authority}";
        }

        public static MvcHtmlString CdnLink(this HtmlHelper html, string folderName, string fileName)
        {
            var cdnLocation = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<ProviderRelationshipsConfiguration>().CdnBaseUrl;

            var trimCharacters = new char[] { '/' };
            return new MvcHtmlString($"{cdnLocation.Trim(trimCharacters)}/{folderName.Trim(trimCharacters)}/{fileName.Trim(trimCharacters)}");
        }
    }
}