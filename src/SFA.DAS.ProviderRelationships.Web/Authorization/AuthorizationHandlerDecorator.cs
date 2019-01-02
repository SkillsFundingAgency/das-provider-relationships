using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Authorization;
using SFA.DAS.AutoConfiguration;

namespace SFA.DAS.ProviderRelationships.Web.Authorization
{
    public class AuthorizationHandlerDecorator : IAuthorizationHandler
    {
        public string Namespace => _authorizationHandler.Namespace;
        
        private readonly IAuthorizationHandler _authorizationHandler;
        private readonly IEnvironmentService _environmentService;

        public AuthorizationHandlerDecorator(IAuthorizationHandler authorizationHandler, IEnvironmentService environmentService)
        {
            _authorizationHandler = authorizationHandler;
            _environmentService = environmentService;
        }

        public async Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            var authorizationResult = new AuthorizationResult();
            
            if (!_environmentService.IsCurrent(DasEnv.LOCAL))
            {
                authorizationResult = await _authorizationHandler.GetAuthorizationResult(options, authorizationContext).ConfigureAwait(false);
            }

            return authorizationResult;
        }
    }
}