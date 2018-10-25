using System;
using System.Web;
using SFA.DAS.Authorization;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.HashingService;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Web.Routing;

namespace SFA.DAS.ProviderRelationships.Web.Authorization
{
    public class AuthorizationContextProvider : IAuthorizationContextProvider
    {
        private readonly HttpContextBase _httpContext;
        private readonly IHashingService _hashingService;
        private readonly IAuthenticationService _authenticationService;

        public AuthorizationContextProvider(HttpContextBase httpContext, IHashingService hashingService, IAuthenticationService authenticationService)
        {
            _httpContext = httpContext;
            _hashingService = hashingService;
            _authenticationService = authenticationService;
        }

        public IAuthorizationContext GetAuthorizationContext()
        {
            var authorizationContext = new AuthorizationContext();
            var accountHashedId = GetAccountHashedId();
            var accountId = accountHashedId == null ? null : GetAccountId(accountHashedId);
            var userRef = GetUserRef();
            
            authorizationContext.Set("AccountHashedId", accountHashedId);
            authorizationContext.Set("AccountId", accountId);
            authorizationContext.Set("UserRef", userRef);

            return authorizationContext;
        }
        
        private string GetAccountHashedId()
        {
            if (!_httpContext.Request.RequestContext.RouteData.Values.TryGetValue(RouteDataKeys.AccountHashedId, out var accountHashedId))
            {
                return null;
            }

            return (string)accountHashedId;
        }

        private long? GetAccountId(string accountHashedId)
        {
            if (!_hashingService.TryDecodeValue(accountHashedId, out var accountId))
            {
                throw new UnauthorizedAccessException();
            }

            return accountId;
        }

        private Guid? GetUserRef()
        {
            if (!_authenticationService.IsUserAuthenticated())
            {
                return null;
            }

            if (!_authenticationService.TryGetCurrentUserClaimValue(DasClaimTypes.Id, out var userRefClaimValue))
            {
                throw new UnauthorizedAccessException();
            }

            if (!Guid.TryParse(userRefClaimValue, out var userRef))
            {
                throw new UnauthorizedAccessException();
            }

            return userRef;
        }
    }
}