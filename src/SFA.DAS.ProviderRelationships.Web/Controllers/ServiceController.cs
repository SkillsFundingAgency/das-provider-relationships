using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
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
        [Route("{accountId}/signout")]
        [AllowAnonymous]
        public ActionResult SignOut()
        {
            _authenticationService.SignOutUser();
            
            _authenticationService.TryGetCurrentUserClaimValue("id_token", out var idToken);

            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = "signoutcleanup",
                AllowRefresh = true
            };
            authenticationProperties.Dictionary.Clear();
            authenticationProperties.Dictionary.Add("id_token",idToken);
            HttpContext.GetOwinContext().Authentication.SignOut(authenticationProperties, CookieAuthenticationDefaults.AuthenticationType, OpenIdConnectAuthenticationDefaults.AuthenticationType);

            return new RedirectResult(_authenticationUrls.LogoutEndpoint);
        }

        [Route("signoutcleanup")]
        public void SignOutCleanup()
        {
            _authenticationService.SignOutUser();
        }
    }
}