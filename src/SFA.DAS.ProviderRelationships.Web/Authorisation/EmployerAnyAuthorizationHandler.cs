namespace SFA.DAS.ProviderRelationships.Web.Authorisation;

public class EmployerAnyAuthorizationHandler : AuthorizationHandler<EmployerAnyRoleRequirement>
{
    private readonly IEmployerAccountAuthorisationHandler _employerAccountAuthorizationHandler;

    public EmployerAnyAuthorizationHandler(IEmployerAccountAuthorisationHandler employerAccountAuthorizationHandler)
    {
        _employerAccountAuthorizationHandler = employerAccountAuthorizationHandler;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerAnyRoleRequirement requirement)
    {
        if (!_employerAccountAuthorizationHandler.IsEmployerAuthorised(context, allowAllUserRoles: true).GetAwaiter().GetResult())
        {
            return Task.CompletedTask;
        }

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}