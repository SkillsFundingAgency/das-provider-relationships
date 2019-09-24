using System.Security.Claims;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Upn(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Identity?.Upn();
        }
    }
}