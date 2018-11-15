using System.Security.Claims;
using System.Web;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    public class OwinAuthenticationService : IAuthenticationService
    {
        public string GetCurrentUserClaimValue(string key)
        {
            var claimsIdentity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var claim = claimsIdentity.FindFirst(key);
            
            return claim.Value;
        }

        public bool IsUserAuthenticated()
        {
            //todo: is using HttpContext going to kill self-hosting?
            return HttpContext.Current.GetOwinContext().Authentication.User.Identity.IsAuthenticated;
            // in .net core: HttpContext.Authentication
        }

        public void SignOutUser()
        {
            var owinContext = HttpContext.Current.GetOwinContext();
            var authenticationManager = owinContext.Authentication;
            
            authenticationManager.SignOut(/* Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie */"Cookies");
        }
        
        public bool TryGetCurrentUserClaimValue(string key, out string value)
        {
            var claimsIdentity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var claim = claimsIdentity.FindFirst(key);
            
            value = claim?.Value;

            return claim != null;
        }
    }
}