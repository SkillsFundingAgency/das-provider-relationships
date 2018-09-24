using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Authentication;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IAuthenticationService _authenticationService;
        public HomeController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public ActionResult Index()
        {
            return View(_authenticationService.IsUserAuthenticated());
        }
    }
}