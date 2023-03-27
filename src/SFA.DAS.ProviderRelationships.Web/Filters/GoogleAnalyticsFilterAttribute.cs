using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ProviderRelationships.Web.Authentication;

namespace SFA.DAS.ProviderRelationships.Web.Filters;

public class GoogleAnalyticsFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!(context.Controller is Controller controller))
        {
            return;
        }

        controller.ViewBag.GaData = PopulateGaData(context);

        base.OnActionExecuting(context);
    }

    private static GaData PopulateGaData(ActionExecutingContext context)
    {
        var userId = (context.HttpContext.User.Identity as ClaimsIdentity).FindFirst(c => c.Type == EmployerClaims.IdamsUserIdClaimTypeIdentifier);
        context.RouteData.Values.TryGetValue("AccountHashedId", out var accountHashedId);

        return new GaData {
            UserId = userId?.Value,
            Acc = accountHashedId?.ToString()
        };
    }
}

public class GaData
{
    public string UserId { get; set; }
    public string Vpv { get; set; }
    public string Acc { get; set; }
    public string Org { get; set; }
    public string Ukprn { get; set; }
}