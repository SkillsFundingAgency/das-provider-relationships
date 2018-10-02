using System.Security.Claims;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    public static class ClaimsIdentityExtensions
    {
        /// <returns>Claim value, or null if not found</returns>
        public static string TryGetClaimValue(this ClaimsIdentity identity, string claimType)
        {
            return identity.FindFirst(claimType)?.Value;
        }
    }
}
