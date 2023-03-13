using SFA.DAS.ProviderRelationships.Web.Authorisation.Requirements;

namespace SFA.DAS.ProviderRelationships.Web.Authorisation.Handlers;

public class EmployerAllRolesAuthorizationHandler : AuthorizationHandler<EmployerAccountAllRolesRequirement>
{
    private readonly IEmployerAccountAuthorisationHandler _handler;

    public EmployerAllRolesAuthorizationHandler(IEmployerAccountAuthorisationHandler handler)
    {
        _handler = handler;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerAccountAllRolesRequirement requirement)
    {
        if (!await _handler.IsEmployerAuthorised(context, true))
        {
            return;
        }

        context.Succeed(requirement);
    }
}