namespace SFA.DAS.ProviderRelationships.Web.Authorisation
{
    public class EmployerViewerAuthorizationHandler : AuthorizationHandler<EmployerViewerRoleRequirement>
    {
        private readonly IEmployerAccountAuthorizationHandler _employerAccountAuthorizationHandler;

        public EmployerViewerAuthorizationHandler(IEmployerAccountAuthorizationHandler employerAccountAuthorizationHandler)
        {
            _employerAccountAuthorizationHandler = employerAccountAuthorizationHandler;
        }
    
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerViewerRoleRequirement requirement)
        {
            if (!_employerAccountAuthorizationHandler.IsEmployerAuthorised(context, EmployerUserAuthorisationRole.Viewer))
            {
                return Task.CompletedTask;
            }
            
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}