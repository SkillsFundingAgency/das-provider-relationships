using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderRelationships.Web.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnvironmentService _environmentService;
        private readonly IEmployerUrls _employerUrls;

        public HomeController(IEnvironmentService environmentService, IEmployerUrls employerUrls)
        {
            _environmentService = environmentService;
            _employerUrls = employerUrls;
        }

        [Route]
        public ActionResult Index(string accountHashedId = null)
        {
            if (_environmentService.IsCurrent(DasEnv.LOCAL))
            {
                return RedirectToAction("Index", "AccountProviders", new { accountHashedId = accountHashedId ?? "JRML7V" });
            }

            return Redirect(_employerUrls.Homepage());
        }

        /*[Route("/signout")]
        [Route("{accountId}/signout")]
        [AllowAnonymous]
        public async Task<ActionResult> SignOutBye()
        {
            var idToken = await HttpContext.GetTokenAsync("id_token");
            await Task.CompletedTask;
            
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = "signoutcleanup",
                AllowRefresh = true
            };
            authenticationProperties.Dictionary.Clear();
            authenticationProperties.Dictionary.Add("id_token",idToken);
            
            //return SignOut(authenticationProperties, CookieAuthenticationDefaults.AuthenticationType, OpenIdConnectAuthenticationDefaults.AuthenticationType)
            return await SignOut();
        }*/
    }
}