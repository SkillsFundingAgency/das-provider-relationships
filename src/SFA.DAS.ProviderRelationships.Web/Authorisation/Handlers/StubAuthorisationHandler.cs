using SFA.DAS.ProviderRelationships.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.Authorisation.Handlers;

public class StubAuthorisationHandler : IEmployerAccountAuthorisationHandler
{
    public Task<bool> IsEmployerAuthorised(AuthorizationHandlerContext context, bool allowAllUserRoles)
    {
        return Task.FromResult(true);
    }

    public Task<bool> CheckUserAccountAccess(ClaimsPrincipal user, EmployerUserRole userRoleRequired)
    {
        return Task.FromResult(true);
    }
}