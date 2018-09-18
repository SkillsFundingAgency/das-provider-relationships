using System.Linq;
using System.Security.Claims;
using System.Web;
//using IdentityModel.Client;
//using SFA.DAS.EmployerUsers.WebClientComponents;
//using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    //todo: clean up the commented out code when we know it's not needed

    /// <remarks>
    /// in EmployerCommitments this class still has its old name of OwinWrapper
    /// </remarks>
    public class OwinAuthenticationService : IAuthenticationService
    {
        //todo: extract authentication config into own class, and use to compose service config: that way we could put this class in a common package, and use it in EAS, employercommitments and here
        //private readonly ProviderRelationshipsConfiguration _configuration;

        //public OwinAuthenticationService(ProviderRelationshipsConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        /// <returns>Claim value, or null if not found (EAS & EC return "" if not found)</returns>
        public string GetCurrentUserClaimValue(string key)
        {
            //todo: do we throw if not found and still have a Try.. method? or return null if not found?
            return ((ClaimsIdentity)HttpContext.Current.User.Identity).Claims.FirstOrDefault(c => c.Type == key)?.Value;
        }

        //public bool IsUserAuthenticated()
        //{
        //    //todo: is using HttpContext going to kill self-hosting?
        //    return HttpContext.Current.GetOwinContext().Authentication.User.Identity.IsAuthenticated;
        //}

        //GetOwinContext requires Microsoft.Owin.Host.SystemWeb package

        //public void SignOutUser()
        //{
        //    var owinContext = HttpContext.Current.GetOwinContext();
        //    var authenticationManager = owinContext.Authentication;

        //    authenticationManager.SignOut("Cookies");
        //}

        //public bool TryGetCurrentUserClaimValue(string key, out string value)
        //{
        //    var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
        //    var claim = identity?.Claims.FirstOrDefault(c => c.Type == key);

        //    value = claim?.Value;

        //    return value != null;
        //}

        //public async Task UpdateClaims()
        //{
        //    var constants = new AuthenticationUrls(_configuration.Identity);
        //    var userInfoEndpoint = constants.UserInfoEndpoint();
        //    var accessToken = GetClaimValue("access_token");
        //    var userInfoClient = new UserInfoClient(new Uri(userInfoEndpoint), accessToken);
        //    var userInfo = await userInfoClient.GetAsync();
        //    var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;

        //    foreach (var claim in userInfo.Claims.ToList())
        //    {
        //        if (claim.Item1.Equals(DasClaimTypes.Email))
        //        {
        //            var emailClaim = identity.Claims.FirstOrDefault(c => c.Type == "email");
        //            var emailClaim2 = identity.Claims.FirstOrDefault(c => c.Type == DasClaimTypes.Email);

        //            identity.RemoveClaim(emailClaim);
        //            identity.RemoveClaim(emailClaim2);
        //            identity.AddClaim(new Claim("email", claim.Item2));
        //            identity.AddClaim(new Claim(DasClaimTypes.Email, claim.Item2));
        //        }

        //        if (claim.Item1.Equals(DasClaimTypes.RequiresVerification))
        //        {
        //            var requiresValidationClaim =
        //                identity.Claims.FirstOrDefault(c => c.Type == DasClaimTypes.RequiresVerification);

        //            if (requiresValidationClaim != null)
        //            {
        //                identity.RemoveClaim(requiresValidationClaim);
        //            }
        //            identity.AddClaim(new Claim(DasClaimTypes.RequiresVerification, claim.Item2));
        //        }
        //    }
        //}
    }
}