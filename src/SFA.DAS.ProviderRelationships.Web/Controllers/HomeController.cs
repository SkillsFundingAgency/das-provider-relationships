using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Authentication.Interfaces;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [RoutePrefix("accounts/{hashedAccountId}")]
    [Authorize]
    public class HomeController : Controller
    {
        private IAuthenticationService _authenticationService;
        public HomeController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [Route]
        public ActionResult Index()
        {
            return View();
        }
    }
}