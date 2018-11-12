using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Routing;

namespace SFA.DAS.ProviderRelationships.Urls
{
    public class UrlsViewBagFilter : ActionFilterAttribute
    {
//        private readonly AccountUrls _accountUrls;
//        private readonly Func<IEmployerUrls> _getApprenticeshipUrls;
//
//        public AccountUrlsViewBagFilter(AccountUrls accountUrls, Func<IEmployerUrls> getApprenticeshipUrls)
//        {
//            // do we want to combine these into 1, or keep authentication (account)) urls separate for easier reuse of authentication code?
//            _accountUrls = accountUrls;
//            _getApprenticeshipUrls = getApprenticeshipUrls;
//        }

//        private readonly IContainer _container;
//
//        public UrlsViewBagFilter(IContainer container)
//        {
//            _container = container;
//        }

        private readonly IViewUrls _viewUrls;

        public UrlsViewBagFilter(IViewUrls viewUrls)
        {
            _viewUrls = viewUrls;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _viewUrls.AccountHashedId = (string)filterContext.RouteData.Values[RouteDataKeys.AccountHashedId];

            filterContext.Controller.ViewBag.Urls = _viewUrls;

//            var employerUrlsArgs = new ExplicitArguments();
//            employerUrlsArgs.Set((string)filterContext.RouteData.Values[RouteDataKeys.AccountHashedId]);
//            
//            filterContext.Controller.ViewBag.Urls = _container.GetInstance<IEmployerUrls>(employerUrlsArgs);

//            filterContext.Controller.ViewBag.ChangePasswordUrl = _accountUrls.ChangePasswordUrl;
//            filterContext.Controller.ViewBag.ChangeEmailUrl = _accountUrls.ChangeEmailUrl;
//
//            var apprenticeshipUrls = _getApprenticeshipUrls();
//
//            var accountHashedId = (string)filterContext.RouteData.Values[RouteDataKeys.AccountHashedId];
//
//            apprenticeshipUrls.AccountHashedId = accountHashedId;
//            filterContext.Controller.ViewBag.Urls = apprenticeshipUrls;
        }
    }
}
