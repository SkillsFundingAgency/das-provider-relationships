using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.Authorisation;

public class EmployerViewerAuthorizationHandler : AuthorizationHandler<EmployerViewerRoleRequirement>
{
    private readonly IEmployerAccountAuthorisationHandler _employerAccountAuthorizationHandler;

    public EmployerViewerAuthorizationHandler(IEmployerAccountAuthorisationHandler employerAccountAuthorizationHandler)
    {
        _employerAccountAuthorizationHandler = employerAccountAuthorizationHandler;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerViewerRoleRequirement requirement)
    {
        if (!_employerAccountAuthorizationHandler.IsEmployerAuthorised(context, EmployerUserRole.Viewer))
        {
            return Task.CompletedTask;
        }
            
        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}