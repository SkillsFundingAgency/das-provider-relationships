using System;
using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Routing;

namespace SFA.DAS.ProviderRelationships.Urls
{
    public class UrlsViewBagFilter : ActionFilterAttribute
    {
        private readonly Func<IViewUrls> _getViewUrls;

        public UrlsViewBagFilter(Func<IViewUrls> getViewUrls)
        {
            _getViewUrls = getViewUrls;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var viewUrls = _getViewUrls();

            viewUrls.AccountHashedId = (string)filterContext.RouteData.Values[RouteDataKeys.AccountHashedId];

            filterContext.Controller.ViewBag.Urls = viewUrls;
        }
    }
}
