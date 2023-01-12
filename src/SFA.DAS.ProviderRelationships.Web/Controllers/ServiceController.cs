
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderRelationships.Web.Authentication;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthenticationUrls _authenticationUrls;

        public ServiceController(IAuthenticationService authenticationService, IAuthenticationUrls authenticationUrls)
        {
            _authenticationService = authenticationService;
            _authenticationUrls = authenticationUrls;
        }

        [Route("signout")]
        public ActionResult SignOut()
        {
            _authenticationService.SignOutUser();

            return new RedirectResult(_authenticationUrls.LogoutEndpoint);
        }

        [Route("signoutcleanup")]
        public void SignOutCleanup()
        {
            _authenticationService.SignOutUser();
        }
    }
}