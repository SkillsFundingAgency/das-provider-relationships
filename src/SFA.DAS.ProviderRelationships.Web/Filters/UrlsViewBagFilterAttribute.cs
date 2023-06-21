using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.ProviderRelationships.Web.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Filters;

public class UrlsViewBagFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var employerUrls =  context.HttpContext.RequestServices.GetRequiredService<IEmployerUrls>();

        var accountHashedId = (string)context.RouteData.Values[RouteValueKeys.AccountHashedId];
            
        employerUrls.Initialize(accountHashedId);

        if (context.Controller is Controller controller)
        {
            controller.ViewBag.EmployerUrls = employerUrls;
        }
    }
}