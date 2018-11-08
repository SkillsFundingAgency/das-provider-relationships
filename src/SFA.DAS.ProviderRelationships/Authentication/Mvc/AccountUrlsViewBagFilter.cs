using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Urls;

namespace SFA.DAS.ProviderRelationships.Authentication.Mvc
{
    public class AccountUrlsViewBagFilter : ActionFilterAttribute
    {
        private readonly AccountUrls _accountUrls;
        private readonly IApprenticeshipUrls _apprenticeshipUrls;

        public AccountUrlsViewBagFilter(AccountUrls accountUrls, IApprenticeshipUrls apprenticeshipUrls)
        {
            //these will be combined into authenticationUrls
            //todo: there is also already an AuthenticationUrls in prorel. combine all into 1?
            _accountUrls = accountUrls;
            _apprenticeshipUrls = apprenticeshipUrls;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.ChangePasswordUrl = _accountUrls.ChangePasswordUrl;
            filterContext.Controller.ViewBag.ChangeEmailUrl = _accountUrls.ChangeEmailUrl;

            //_apprenticeshipUrls.UrlHelper = new UrlHelper(filterContext.RequestContext);
            // not gonna have intellisense support when calling off urls!?
            // could get intellisense by viewmodels having a base containing the urls!?
            // helper Urls => (ApprenticeshipUrls)ViewBag.Urls
            filterContext.Controller.ViewBag.Urls = _apprenticeshipUrls;
        }
    }
}
