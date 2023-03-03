namespace SFA.DAS.ProviderRelationships.Web.Authorisation
{
    public class EmployerOwnerAuthorizationHandler : AuthorizationHandler<EmployerOwnerRoleRequirement>
    {
        private readonly IEmployerAccountAuthorizationHandler _employerAccountAuthorizationHandler;

        public EmployerOwnerAuthorizationHandler(IEmployerAccountAuthorizationHandler employerAccountAuthorizationHandler)
        {
            _employerAccountAuthorizationHandler = employerAccountAuthorizationHandler;
        }
    
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerOwnerRoleRequirement requirement)
        {
            if (!_employerAccountAuthorizationHandler.IsEmployerAuthorised(context, EmployerUserAuthorisationRole.Owner))
            {
                return Task.CompletedTask;
            }
            
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}