using System;
using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    //attribute as global filter?
    public class AccountLinksFilter : ActionFilterAttribute
    {
        private readonly Func<(ProviderRelationshipsConfiguration, IAuthenticationUrls)> _provideDependencies;

        public AccountLinksFilter(Func<(ProviderRelationshipsConfiguration, IAuthenticationUrls)> provideDependencies)
        {
            _provideDependencies = provideDependencies;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var (providerRelationshipsConfig, authenticationUrls) = _provideDependencies();

            var urlHelper = new UrlHelper();
            // the second interpolated expression is the return url (we send them back to MA)
            filterContext.Controller.ViewBag.ChangePasswordLink = $"{authenticationUrls.ChangePasswordLink}{urlHelper.Encode(providerRelationshipsConfig.EmployerPortalBaseUrl.TrimEnd('/') + "/service/password/change")}";
            filterContext.Controller.ViewBag.ChangeEmailLink = $"{authenticationUrls.ChangeEmailLink}{urlHelper.Encode(providerRelationshipsConfig.EmployerPortalBaseUrl.TrimEnd('/') + "/service/email/change")}";
        }
    }
}
