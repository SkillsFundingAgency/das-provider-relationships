namespace SFA.DAS.ProviderRelationships.Web.Authorisation;

public class EmployerOwnerAuthorizationHandler : AuthorizationHandler<EmployerOwnerRoleRequirement>
{
    private readonly IEmployerAccountAuthorisationHandler _employerAccountAuthorizationHandler;

    public EmployerOwnerAuthorizationHandler(IEmployerAccountAuthorisationHandler employerAccountAuthorizationHandler)
    {
        _employerAccountAuthorizationHandler = employerAccountAuthorizationHandler;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerOwnerRoleRequirement requirement)
    {
        if (!(await _employerAccountAuthorizationHandler.IsEmployerAuthorised(context, false)))
        {
            return;
        }
        context.Succeed(requirement);
    }
}