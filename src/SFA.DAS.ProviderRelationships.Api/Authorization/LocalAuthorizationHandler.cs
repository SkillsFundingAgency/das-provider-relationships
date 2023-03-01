namespace SFA.DAS.ProviderRelationships.Api.Authorization;

public class LocalAuthorizationHandler : AuthorizationHandler<NoneRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NoneRequirement requirement)
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}