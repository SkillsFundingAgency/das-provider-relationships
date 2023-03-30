using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers;

[Route("error")]
public class ErrorController : Controller
{
    [Route("403")]
    public IActionResult AccessDenied()
    {
        Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return View();
    }

    [Route("404")]
    public IActionResult PageNotFound()
    {
        Response.StatusCode = (int)HttpStatusCode.NotFound;

        return View();
    }
    
    [Route("500")]
    public IActionResult Error()
    {
        Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return View();
    }
}