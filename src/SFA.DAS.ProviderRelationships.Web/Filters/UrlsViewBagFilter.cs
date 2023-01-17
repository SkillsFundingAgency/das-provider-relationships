using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.ProviderRelationships.Web.Urls;

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
            var accountHashedId = (string)filterContext.RouteData.Values[RouteValueKeys.AccountHashedId];
            
            employerUrls.Initialize(accountHashedId);

            if (filterContext.Controller is Controller controller)
            {
                controller.ViewBag.EmployerUrls = employerUrls;
            }
        }
    }
}