using SFA.DAS.ProviderRelationships.Web.Authorisation;

namespace SFA.DAS.ProviderRelationships.Web.Authentication;

public class EmployerAccountAllRolesAuthorizationHandler : AuthorizationHandler<EmployerAccountAllRolesRequirement>
{
    private readonly IEmployerAccountAuthorisationHandler _handler;

    public EmployerAccountAllRolesAuthorizationHandler(IEmployerAccountAuthorisationHandler handler)
    {
        _handler = handler;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerAccountAllRolesRequirement requirement)
    {
        if (!(await _handler.IsEmployerAuthorised(context, true)))
        {
            return;
        }

        context.Succeed(requirement);

        return;
    }
}