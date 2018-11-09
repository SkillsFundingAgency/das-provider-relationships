using System;
using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Routing;
using SFA.DAS.ProviderRelationships.Urls;

namespace SFA.DAS.ProviderRelationships.Authentication.Mvc
{
    //todo: this was authentication only - do we want to move this filter out of authentication
    // or split into 2 - 1 for authentication and 1 for employer urls?
    public class AccountUrlsViewBagFilter : ActionFilterAttribute
    {
        private readonly AccountUrls _accountUrls;
        private readonly Func<IEmployerUrls> _getApprenticeshipUrls;

        public AccountUrlsViewBagFilter(AccountUrls accountUrls, Func<IEmployerUrls> getApprenticeshipUrls)
        {
            // do we want to combine these into 1, or keep authentication (account)) urls separate for easier reuse of authentication code?
            _accountUrls = accountUrls;
            _getApprenticeshipUrls = getApprenticeshipUrls;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.ChangePasswordUrl = _accountUrls.ChangePasswordUrl;
            filterContext.Controller.ViewBag.ChangeEmailUrl = _accountUrls.ChangeEmailUrl;

            var apprenticeshipUrls = _getApprenticeshipUrls();

            var accountHashedId = (string)filterContext.RouteData.Values[RouteDataKeys.AccountHashedId];

            apprenticeshipUrls.AccountHashedId = accountHashedId;
            filterContext.Controller.ViewBag.Urls = apprenticeshipUrls;
        }
    }
}
