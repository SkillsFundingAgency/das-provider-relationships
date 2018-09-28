using System.Web;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    /// <remarks>
    /// in EmployerCommitments this class still has its old name of OwinWrapper
    /// </remarks>
    public class OwinAuthenticationService : IAuthenticationService
    {
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
    }
}