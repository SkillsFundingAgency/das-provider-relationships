using System.Security.Claims;
using System.Web;
using SFA.DAS.ProviderRelationships.Authentication.Interfaces;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    /// <remarks>
    /// in EmployerCommitments this class still has its old name of OwinWrapper
    /// </remarks>
    public class OwinAuthenticationService : IAuthenticationService
    {
        /// <returns>Claim value, or null if not found (EAS & EC return "" if not found)</returns>
        public string TryGetCurrentUserClaimValue(string key)
        {
            return ((ClaimsIdentity)HttpContext.Current.User.Identity).GetClaimValue(key);
        }

        public bool IsUserAuthenticated()
        {
            //todo: is using HttpContext going to kill self-hosting?
            return HttpContext.Current.GetOwinContext().Authentication.User.Identity.IsAuthenticated;
        }

        public void SignOutUser()
        {
            var owinContext = HttpContext.Current.GetOwinContext();
            var authenticationManager = owinContext.Authentication;
            
            authenticationManager.SignOut(/* Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie */"Cookies");
        }
    }
}