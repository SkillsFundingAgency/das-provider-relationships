using System;
using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Web.Filters
{
    public class GoogleAnalyticsViewBagFilter : ActionFilterAttribute
    {
        private readonly Func<GoogleAnalyticsConfiguration> _configuration;

        public GoogleAnalyticsViewBagFilter(Func<GoogleAnalyticsConfiguration> configuration)
        {
            _configuration = configuration;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.GoogleAnalyticsConfiguration = _configuration();
        }
    }
}