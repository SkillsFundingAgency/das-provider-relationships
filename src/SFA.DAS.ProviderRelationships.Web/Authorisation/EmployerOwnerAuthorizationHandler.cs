namespace SFA.DAS.ProviderRelationships.Web.Authorisation
{
    public class EmployerOwnerAuthorizationHandler : AuthorizationHandler<EmployerOwnerRoleRequirement>
    {
        private readonly IEmployerAccountAuthorisationHandler _employerAccountAuthorizationHandler;

        public EmployerOwnerAuthorizationHandler(IEmployerAccountAuthorisationHandler employerAccountAuthorizationHandler)
        {
            _employerAccountAuthorizationHandler = employerAccountAuthorizationHandler;
        }
    
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployerOwnerRoleRequirement requirement)
        {
            if (!_employerAccountAuthorizationHandler.IsEmployerAuthorised(context, EmployerUserRole.Owner))
            {
                return Task.CompletedTask;
            }
            
            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}