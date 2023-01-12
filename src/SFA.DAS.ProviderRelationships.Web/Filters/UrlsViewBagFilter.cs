using System;
using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.ProviderRelationships.Web.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Filters
{
    public class UrlsViewBagFilter : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        private readonly Func<IEmployerUrls> _employerUrls;

        public UrlsViewBagFilter(Func<IEmployerUrls> employerUrls)
        {
            _employerUrls = employerUrls;
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext filterContext)
        {
            var employerUrls = _employerUrls();
            var accountHashedId = (string)filterContext.RouteData.Values[RouteValueKeys.AccountHashedId];
            
            employerUrls.Initialize(accountHashedId);

            Microsoft.AspNetCore.Mvc.Controller.ViewBag.EmployerUrls = employerUrls;
        }
    }
}