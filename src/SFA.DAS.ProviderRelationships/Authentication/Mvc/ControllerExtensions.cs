using System.Security.Claims;
using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Authentication.Mvc
{
    public static class ControllerExtensions
    {
        /// <param name="key">Use values from DasClaimTypes, e.g. DasClaimTypes.Id</param>
        /// <returns>Claim value, or null if not found</returns>
        public static string TryGetClaimValue(this Controller controller, string key)
        {
            // should be core/self hosting friendly (https://davidpine.net/blog/principal-architecture-changes/)
            return ((ClaimsPrincipal)controller.User).FindFirst(key)?.Value;
        }
    }
}
