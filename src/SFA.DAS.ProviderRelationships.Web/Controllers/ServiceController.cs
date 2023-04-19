using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using SFA.DAS.GovUK.Auth.Models;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.ProviderRelationships.Web.RouteValues;

namespace SFA.DAS.ProviderRelationships.Web.Controllers;

public class ServiceController : Controller
{
    private readonly IConfiguration _config;
    private readonly IStubAuthenticationService _stubAuthenticationService;

    public ServiceController(IConfiguration config, IStubAuthenticationService stubAuthenticationService)
    {
        _config = config;
        _stubAuthenticationService = stubAuthenticationService;
    }
    
    [AllowAnonymous]
    [Route("signout", Name = RouteNames.SignOut)]
    public async Task<IActionResult> SignOutEmployer()
    {
        var idToken = await HttpContext.GetTokenAsync("id_token");

        var authenticationProperties = new AuthenticationProperties();
        authenticationProperties.Parameters.Clear();
        authenticationProperties.Parameters.Add("id_token", idToken);

        return SignOut(authenticationProperties, CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [AllowAnonymous]
    [Route("signoutcleanup",Name=RouteNames.SignOutCleanup)]
    public async Task<IActionResult> SignOutCleanup()
    {
        var idToken = await HttpContext.GetTokenAsync("id_token");

        var authenticationProperties = new AuthenticationProperties();
        authenticationProperties.Parameters.Clear();
        authenticationProperties.Parameters.Add("id_token", idToken);

        return SignOut(authenticationProperties, CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
    }
    
    
#if DEBUG
    [AllowAnonymous]
    [HttpGet]
    [Route("SignIn-Stub")]
    public IActionResult SigninStub()
    {
        return View("SigninStub", new List<string>{_config["StubId"],_config["StubEmail"]});
    }
    [AllowAnonymous]
    [HttpPost]
    [Route("SignIn-Stub")]
    public IActionResult SigninStubPost()
    {
        _stubAuthenticationService?.AddStubEmployerAuth(Response.Cookies, new StubAuthUserDetails
        {
            Email = _config["StubEmail"],
            Id = _config["StubId"]
        }, true);

        return RedirectToRoute("Signed-in-stub");
    }

    [Authorize]
    [HttpGet]
    [Route("signed-in-stub", Name = "Signed-in-stub")]
    public IActionResult SignedInStub()
    {
        return View();
    }
#endif
}