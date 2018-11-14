using System.Web.Mvc;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Web.Extensions;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerFeature.ProviderRelationships)]
    [RoutePrefix("service")]
    public class ServiceController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public ServiceController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [Route("signout")]
        public ActionResult SignOut()
        {
            _authenticationService.SignOutUser();

            var url = Url.EmployerPortalAction("service/signout");

            return new RedirectResult(url);
        }
    }
}