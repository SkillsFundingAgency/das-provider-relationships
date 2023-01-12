using System.Security.Claims;
using System.Web;
using SFA.DAS.ProviderRelationships.Web;

namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public class OwinAuthenticationService : IAuthenticationService
    {
        public string GetCurrentUserClaimValue(string key)
        {
            var claimsIdentity = (ClaimsIdentity)HttpContextHelper.Current.User.Identity;
            var claim = claimsIdentity.FindFirst(key);
            
            return claim.Value;
        }

        public bool IsUserAuthenticated()
        {
            //todo: is using HttpContext going to kill self-hosting?
            return HttpContextHelper.Current.GetOwinContext().Authentication.User.Identity.IsAuthenticated;
            // in .net core: HttpContext.Authentication
        }

        public void SignOutUser()
        {
            var owinContext = HttpContextHelper.Current.GetOwinContext();
            var authenticationManager = owinContext.Authentication;
            
            authenticationManager.SignOut(/* Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie */"Cookies");
        }
        
        public bool TryGetCurrentUserClaimValue(string key, out string value)
        {
            var claimsIdentity = (ClaimsIdentity)HttpContextHelper.Current.User.Identity;
            var claim = claimsIdentity.FindFirst(key);
            
            value = claim?.Value;

            return claim != null;
        }
    }
}