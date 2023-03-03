namespace SFA.DAS.ProviderRelationships.Web.Authorisation;

public interface IEmployerAccountAuthorizationHandler
{
    bool IsEmployerAuthorised(AuthorizationHandlerContext context, EmployerUserAuthorisationRole allowedRole);
}