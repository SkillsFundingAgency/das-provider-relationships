using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [RoutePrefix("service")]
    public class ServiceController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmployerUrls _employerUrls;

        public ServiceController(IAuthenticationService authenticationService, IEmployerUrls employerUrls)
        {
            _authenticationService = authenticationService;
            _employerUrls = employerUrls;
        }

        [Route("signout")]
        public ActionResult SignOut()
        {
            _authenticationService.SignOutUser();

            return new RedirectResult(_employerUrls.SignOut());
        }
    }
}