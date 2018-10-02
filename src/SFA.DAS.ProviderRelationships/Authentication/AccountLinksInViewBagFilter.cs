using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    //attribute as global filter?
    public class AccountLinksInViewBagFilter : ActionFilterAttribute
    {
        private readonly AccountLinks _accountLinks;

        public AccountLinksInViewBagFilter(AccountLinks accountLinks)
        {
            _accountLinks = accountLinks;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.ChangePasswordLink = _accountLinks.ChangePasswordLink;
            filterContext.Controller.ViewBag.ChangeEmailLink = _accountLinks.ChangeEmailLink;
        }
    }
}
