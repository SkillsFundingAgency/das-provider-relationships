using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [RoutePrefix("service")]
    public class ServiceController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IApprenticeshipUrls _apprenticeshipUrls;

        public ServiceController(IAuthenticationService authenticationService, IApprenticeshipUrls apprenticeshipUrls)
        {
            _authenticationService = authenticationService;
            _apprenticeshipUrls = apprenticeshipUrls;
        }

        [Route("signout")]
        public ActionResult SignOut()
        {
            _authenticationService.SignOutUser();

            var url = _apprenticeshipUrls.EmployerPortalAction("service/signout");

            return new RedirectResult(url);
        }
    }
}