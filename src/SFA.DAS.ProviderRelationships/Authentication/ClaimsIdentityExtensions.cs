using System.Linq;
using System.Security.Claims;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    public static class ClaimsIdentityExtensions
    {
        public static string GetClaimValue(this ClaimsIdentity identity, string claimType)
        {
            return identity.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
        }
    }
}
