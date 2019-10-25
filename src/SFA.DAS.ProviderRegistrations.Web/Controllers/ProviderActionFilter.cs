using Microsoft.AspNetCore.Mvc.Filters;

namespace SFA.DAS.ProviderRegistrations.Web.Controllers
{
    public class ProviderActionFilter : ActionFilterAttribute
    {
        private const string ProviderIdKey = "ProviderId";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values.ContainsKey(ProviderIdKey) && typeof(BaseProviderController).IsAssignableFrom(filterContext.Controller.GetType()))
            {
                var providerId = filterContext.RouteData.Values[ProviderIdKey].ToString();
                var controller = ((BaseProviderController)filterContext.Controller);
                controller.ProviderId = providerId;
                controller.ViewBag.ProviderId = providerId;

            }
        }
    }
}
