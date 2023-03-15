using SFA.DAS.ProviderRelationships.Web.Authentication;

namespace SFA.DAS.ProviderRelationships.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(EmployerClaims.IdamsUserIdClaimTypeIdentifier)?.Value;
    }

    public static Guid GetUserRef(this ClaimsPrincipal user)
    {
        return Guid.Parse(GetUserId(user));
    }
}