using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Results;

namespace SFA.DAS.ProviderRegistrations.Web.Authorization
{
    public class ServiceAuthorizationHandler : IAuthorizationHandler
    {
        public string Prefix => "Service.";

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();
            var services = authorizationContext.Get<IEnumerable<string>>(AuthorizationContextKeys.Services);
            
            if (services.All(s => s != ServiceValues.DAA))
            {
                authorizationResult.AddError(new ServiceNotAuthorized());
            }

            return Task.FromResult(authorizationResult);
        }
    }
}