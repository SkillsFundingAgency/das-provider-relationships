using System;
using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Urls;
using SFA.DAS.ProviderRelationships.Web.Routing;

namespace SFA.DAS.ProviderRelationships.Web.Filters
{
    public class UrlsViewBagFilter : ActionFilterAttribute
    {
        private readonly Func<IEmployerUrls> _employerUrls;

        public UrlsViewBagFilter(Func<IEmployerUrls> employerUrls)
        {
            _employerUrls = employerUrls;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var employerUrls = _employerUrls();
            var accountHashedId = (string)filterContext.RouteData.Values[RouteDataKeys.AccountHashedId];
            
            employerUrls.Initialize(accountHashedId);
            
            filterContext.Controller.ViewBag.EmployerUrls = employerUrls;
        }
    }
}