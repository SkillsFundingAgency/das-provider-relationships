using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using SFA.DAS.MA.Shared.UI.Configuration;
using SFA.DAS.MA.Shared.UI.Models;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.ProviderRelationships.Web.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IEmployerUrls EmployerUrls(this IHtmlHelper htmlHelper)
        {
            return (IEmployerUrls)htmlHelper.ViewBag.EmployerUrls;
        }

        public static ICookieBannerViewModel GetCookieBannerViewModel(this IHtmlHelper html)
        {
            var configuration = new EmployerUrlsConfiguration();//todo DependencyResolver.Current.GetService<IEmployerUrlsConfiguration>();

            return new CookieBannerViewModel(new CookieBannerConfiguration
            {
                ManageApprenticeshipsBaseUrl = configuration.EmployerAccountsBaseUrl
            },
            new UserContext
            {
                User = html.ViewContext.HttpContext.User,
                HashedAccountId = html.ViewContext.RouteData.Values[RouteValueKeys.AccountHashedId]?.ToString()
            }
            );
        }

        public static IHeaderViewModel GetHeaderViewModel(this IHtmlHelper html)
        {   
            var configuration = new EmployerUrlsConfiguration();//todo DependencyResolver.Current.GetService<IEmployerUrlsConfiguration>();
            var oidcConfiguration = new OidcConfiguration();//todo DependencyResolver.Current.GetService<IOidcConfiguration>();
            var requestRoot = GetRootUrl(html.ViewContext.HttpContext.Request);

            var headerModel = new HeaderViewModel(new HeaderConfiguration {
                ManageApprenticeshipsBaseUrl = configuration.EmployerAccountsBaseUrl,
                ApplicationBaseUrl = configuration.EmployerAccountsBaseUrl,
                EmployerCommitmentsV2BaseUrl = configuration.EmployerCommitmentsBaseUrl,
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

        public static IFooterViewModel GetFooterViewModel(this IHtmlHelper html)
        {
            var configuration = new EmployerUrlsConfiguration();//todo DependencyResolver.Current.GetService<IEmployerUrlsConfiguration>();

            return new FooterViewModel(new FooterConfiguration {
                ManageApprenticeshipsBaseUrl = configuration.EmployerAccountsBaseUrl,
                AuthenticationAuthorityUrl = configuration.EmployerUsersBaseUrl
            },
            new UserContext {
                User = html.ViewContext.HttpContext.User,
                HashedAccountId = html.ViewContext.RouteData.Values[RouteValueKeys.AccountHashedId]?.ToString()
            }
            );
        }

        private static string GetRootUrl(HttpRequest request)
        {
            var requestUrl = new Uri(request.Host.ToUriComponent());

            return $"{requestUrl.Scheme}://{requestUrl.Authority}";
        }

        public static HtmlString CdnLink(this IHtmlHelper html, string folderName, string fileName)
        {
            var cdnLocation = "config.CdnBaseUrl"; //StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<ProviderRelationshipsConfiguration>().CdnBaseUrl;

            var trimCharacters = new char[] { '/' };
            return new HtmlString($"{cdnLocation.Trim(trimCharacters)}/{folderName.Trim(trimCharacters)}/{fileName.Trim(trimCharacters)}");
        }
    }
}