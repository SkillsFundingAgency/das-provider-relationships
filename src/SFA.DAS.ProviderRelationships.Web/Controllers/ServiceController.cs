using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    public class ServiceController : Controller
    {
        [Route("sign-out")]
        [AllowAnonymous]
        public async Task<IActionResult> SignOutEmployer()
        {
            var idToken = await HttpContext.GetTokenAsync("id_token");

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.Parameters.Clear();
            authenticationProperties.Parameters.Add("id_token",idToken);
            return SignOut(
                authenticationProperties, CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [Route("signoutcleanup")]
        [AllowAnonymous]
        public void SignOutCleanup()
        {
            Response.Cookies.Delete("SFA.DAS.TODO_CookieName.Web.EmployerAuth");//todo
            //_authenticationService.SignOutUser();
        }
    }
}