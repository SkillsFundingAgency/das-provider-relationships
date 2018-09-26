using System.Security.Claims;
using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    public static class ControllerExtensions
    {
        /// <returns>Claim value, or null if not found</returns>
        public static string TryGetClaimValue(this Controller controller, string key)
        {
            // should be core/self hosting friendly (https://davidpine.net/blog/principal-architecture-changes/)
            return ((ClaimsPrincipal)controller.User).FindFirst(key)?.Value;
        }
    }
}
