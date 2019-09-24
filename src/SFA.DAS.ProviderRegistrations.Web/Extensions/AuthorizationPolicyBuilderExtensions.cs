using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ProviderRegistrations.Web.Authorization;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireProviderInRouteMatchesProviderInClaims(this AuthorizationPolicyBuilder builder)
        {
            builder.Requirements.Add(new ProviderRequirement());
            return builder;
        }
    }
}