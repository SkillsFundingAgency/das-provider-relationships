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
            return _environmentService.IsCurrent(DasEnv.LOCAL)
                ? new AuthorizationResult()
                : await _authorizationHandler.GetAuthorizationResult(options, authorizationContext);
        }
    }
}