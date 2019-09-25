using SFA.DAS.Authorization.Errors;

namespace SFA.DAS.ProviderRegistrations.Web.Authorization
{
    public class ServiceNotAuthorized : AuthorizationError
    {
        public ServiceNotAuthorized() : base("Service is not authorized")
        {
        }
    }
}