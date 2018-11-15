using System;
using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Authentication.Mvc
{
    public class AccountUrlsViewBagFilter : ActionFilterAttribute
    {
        private readonly Func<AccountUrls> _accountUrls;

        public AccountUrlsViewBagFilter(Func<AccountUrls> accountUrls)
        {
            _accountUrls = accountUrls;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var accountUrls = _accountUrls();
            
            filterContext.Controller.ViewBag.ChangeEmailUrl = accountUrls.ChangeEmailUrl;
            filterContext.Controller.ViewBag.ChangePasswordUrl = accountUrls.ChangePasswordUrl;
        }
    }
}