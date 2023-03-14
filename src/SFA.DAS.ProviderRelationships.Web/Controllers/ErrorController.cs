using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers;

public class ErrorController : Controller
{
    [Route("accessdenied")]
    public IActionResult AccessDenied()
    {
        Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return View();
    }

    [Route("error")]
    public IActionResult Error()
    {
        Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return View();
    }
}