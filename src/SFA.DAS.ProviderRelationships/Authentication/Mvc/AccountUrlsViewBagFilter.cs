using System;
using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Urls;

namespace SFA.DAS.ProviderRelationships.Authentication.Mvc
{
    public class AccountUrlsViewBagFilter : ActionFilterAttribute
    {
        private readonly AccountUrls _accountUrls;
        private readonly Func<IEmployerUrls> _getApprenticeshipUrls;

        public AccountUrlsViewBagFilter(AccountUrls accountUrls, Func<IEmployerUrls> getApprenticeshipUrls)
        {
            //these will be combined into authenticationUrls
            //todo: there is also already an AuthenticationUrls in prorel. combine all into 1?
            _accountUrls = accountUrls;
            _getApprenticeshipUrls = getApprenticeshipUrls;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.ChangePasswordUrl = _accountUrls.ChangePasswordUrl;
            filterContext.Controller.ViewBag.ChangeEmailUrl = _accountUrls.ChangeEmailUrl;

            var apprenticeshipUrls = _getApprenticeshipUrls();

            //todo: move RouteDataKeys.AccountHashedId down??
            var accountHashedId = (string)filterContext.RouteData.Values["accountHashedId"];

            apprenticeshipUrls.AccountHashedId = accountHashedId;
            // not gonna have intellisense support when calling off urls!?
            // could get intellisense by viewmodels having a base containing the urls!?
            // helper Urls => (ApprenticeshipUrls)ViewBag.Urls
            filterContext.Controller.ViewBag.Urls = apprenticeshipUrls;
        }
    }
}
