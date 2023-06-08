using SFA.DAS.ProviderRelationships.Authorization;
using SFA.DAS.ProviderRelationships.Web.Authorisation.Requirements;

namespace SFA.DAS.ProviderRelationships.Web.Authorisation.Handlers
{
    public class EmployerViewerAuthorizationHandler : AuthorizationHandler<EmployerViewerRoleRequirement>
    {
        private readonly IEmployerAccountAuthorisationHandler _employerAccountAuthorizationHandler;

        public EmployerViewerAuthorizationHandler(IEmployerAccountAuthorisationHandler employerAccountAuthorizationHandler)
        {
            _employerAccountAuthorizationHandler = employerAccountAuthorizationHandler;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerViewerRoleRequirement requirement)
        {
            if (!await _employerAccountAuthorizationHandler.CheckUserAccountAccess(context.User, EmployerUserRole.Viewer))
            {
                return;
            }

            context.Succeed(requirement);
        }
    }
}