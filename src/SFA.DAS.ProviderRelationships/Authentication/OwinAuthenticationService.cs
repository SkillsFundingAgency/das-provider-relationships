using System.Security.Claims;
using System.Web;
using SFA.DAS.ProviderRelationships.Authentication.Interfaces;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    //todo: clean up the commented out code when we know it's not needed

    /// <remarks>
    /// in EmployerCommitments this class still has its old name of OwinWrapper
    /// </remarks>
    public class OwinAuthenticationService : IAuthenticationService
    {
        /// <returns>Claim value, or null if not found (EAS & EC return "" if not found)</returns>
        public string GetCurrentUserClaimValue(string key)
        {
            //todo: do we throw if not found and still have a Try.. method? or return null if not found?
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
            //Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie
            authenticationManager.SignOut("Cookies");
        }

        //public bool TryGetCurrentUserClaimValue(string key, out string value)
        //{
        //    var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
        //    var claim = identity?.Claims.FirstOrDefault(c => c.Type == key);

        //    value = claim?.Value;

        //    return value != null;
        //}
    }
}