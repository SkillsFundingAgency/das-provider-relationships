using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.Authorisation;

public interface IEmployerAccountAuthorisationHandler
{
    bool IsEmployerAuthorised(AuthorizationHandlerContext context, EmployerUserRole allowedRole);
}