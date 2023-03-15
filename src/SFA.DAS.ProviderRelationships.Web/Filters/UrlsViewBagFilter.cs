using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.ProviderRelationships.Web.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Filters;

public class UrlsViewBagFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var employerUrls =  filterContext.HttpContext.RequestServices.GetRequiredService<IEmployerUrls>();

        var accountHashedId = (string)filterContext.RouteData.Values[RouteValueKeys.AccountHashedId];
            
        employerUrls.Initialize(accountHashedId);

        if (filterContext.Controller is Controller controller)
        {
            controller.ViewBag.EmployerUrls = employerUrls;
        }
    }
}