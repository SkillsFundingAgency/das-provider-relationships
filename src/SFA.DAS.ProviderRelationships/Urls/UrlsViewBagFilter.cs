using System;
using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Urls
{
    public class UrlsViewBagFilter : ActionFilterAttribute
    {
        private readonly Func<IEmployerUrls> _urls;

        public UrlsViewBagFilter(Func<IEmployerUrls> urls)
        {
            _urls = urls;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Urls = _urls();
        }
    }
}
