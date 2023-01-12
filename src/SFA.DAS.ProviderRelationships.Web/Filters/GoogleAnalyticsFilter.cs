using System.Security.Claims;
using System.Web.Mvc;
using SFA.DAS.EmployerUsers.WebClientComponents;

namespace SFA.DAS.ProviderRelationships.Web.Filters
{
    public class GoogleAnalyticsFilter : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext filterContext)
        {
            if (!(filterContext.Controller is Controller controller))
            {
                return;
            }

            controller.ViewBag.GaData = PopulateGaData(filterContext);

            base.OnActionExecuting(filterContext);
        }

        private GaData PopulateGaData(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var userId = (context.HttpContext.User.Identity as ClaimsIdentity).FindFirst(c => c.Type == DasClaimTypes.Id);
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
}