using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    //attribute as global filter?
    public class AccountLinksInViewBagFilter : ActionFilterAttribute
    {
        private readonly string _changePasswordLink;
        private readonly string _changeEmailLink;

        public AccountLinksInViewBagFilter(ProviderRelationshipsConfiguration providerRelationshipsConfig, IAuthenticationUrls authenticationUrls)
        {
            var urlHelper = new UrlHelper();
            // the second interpolated expression is the return url (we send them back to MA)
            _changePasswordLink = $"{authenticationUrls.ChangePasswordLink}{urlHelper.Encode(providerRelationshipsConfig.EmployerPortalBaseUrl.TrimEnd('/') + "/service/password/change")}";
            _changeEmailLink = $"{authenticationUrls.ChangeEmailLink}{urlHelper.Encode(providerRelationshipsConfig.EmployerPortalBaseUrl.TrimEnd('/') + "/service/email/change")}";
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.ChangePasswordLink = _changePasswordLink;
            filterContext.Controller.ViewBag.ChangeEmailLink = _changeEmailLink;
        }
    }
}
