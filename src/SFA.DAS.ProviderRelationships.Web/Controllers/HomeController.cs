using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Authentication;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [RoutePrefix("")]
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
            return View(_authenticationService.IsUserAuthenticated());
        }
    }
}