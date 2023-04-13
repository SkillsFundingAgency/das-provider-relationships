using SFA.DAS.ProviderRelationships.Web.RouteValues;

namespace SFA.DAS.ProviderRelationships.Web.Controllers;

[Route("error")]
public class ErrorController : Controller
{
    [Route(RouteNames.AccessDenied)]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [Route(RouteNames.PageNotFound)]
    public IActionResult PageNotFound()
    {
        return View();
    }

    [Route(RouteNames.Error)]
    public IActionResult Error()
    {
        return View();
    }
}