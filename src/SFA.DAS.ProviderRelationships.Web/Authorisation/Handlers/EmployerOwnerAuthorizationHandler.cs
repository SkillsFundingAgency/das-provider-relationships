using SFA.DAS.ProviderRelationships.Authorization;
using SFA.DAS.ProviderRelationships.Web.Authorisation.Requirements;

namespace SFA.DAS.ProviderRelationships.Web.Authorisation.Handlers;

public class EmployerOwnerAuthorizationHandler : AuthorizationHandler<EmployerOwnerRoleRequirement>
{
    private readonly IEmployerAccountAuthorisationHandler _employerAccountAuthorizationHandler;

    public EmployerOwnerAuthorizationHandler(IEmployerAccountAuthorisationHandler employerAccountAuthorizationHandler)
    {
        _employerAccountAuthorizationHandler = employerAccountAuthorizationHandler;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerOwnerRoleRequirement requirement)
    {
        if (!await  _employerAccountAuthorizationHandler.CheckUserAccountAccess(context.User, EmployerUserRole.Owner))
        {
            return;
        }
        context.Succeed(requirement);
    }
}