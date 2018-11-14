using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Authentication.Mvc
{
    public class AccountUrlsViewBagFilter : ActionFilterAttribute
    {
        private readonly AccountUrls _accountUrls;

        public AccountUrlsViewBagFilter(AccountUrls accountUrls)
        {
            _accountUrls = accountUrls;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.ChangeEmailUrl = _accountUrls.ChangeEmailUrl;
            filterContext.Controller.ViewBag.ChangePasswordUrl = _accountUrls.ChangePasswordUrl;
        }
    }
}