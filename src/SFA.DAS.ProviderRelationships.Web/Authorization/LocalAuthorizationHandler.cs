using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Authorization;
using SFA.DAS.AutoConfiguration;

namespace SFA.DAS.ProviderRelationships.Web.Authorization
{
    public class LocalAuthorizationHandler : IAuthorizationHandler
    {
        public string Namespace => _authorizationHandler.Namespace;
        
        private readonly IAuthorizationHandler _authorizationHandler;
        private readonly IEnvironmentService _environmentService;

        public LocalAuthorizationHandler(IAuthorizationHandler authorizationHandler, IEnvironmentService environmentService)
        {
            _authorizationHandler = authorizationHandler;
            _environmentService = environmentService;
        }

        public Task<AuthorizationResult> GetAuthorizationResult(IReadOnlyCollection<string> options, IAuthorizationContext authorizationContext)
        {
            return _environmentService.IsCurrent(DasEnv.LOCAL)
                ? Task.FromResult(new AuthorizationResult())
                : _authorizationHandler.GetAuthorizationResult(options, authorizationContext);
        }
    }
}