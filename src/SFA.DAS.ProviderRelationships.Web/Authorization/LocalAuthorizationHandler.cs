using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.AutoConfiguration;

namespace SFA.DAS.ProviderRelationships.Web.Authorization
{
    public class LocalAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IAuthorizationHandler _authorizationHandler;
        private readonly IEnvironmentService _environmentService;
        
        public string Prefix => _authorizationHandler.Prefix;

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