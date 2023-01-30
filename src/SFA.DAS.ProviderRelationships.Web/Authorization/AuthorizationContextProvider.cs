using System;
using Microsoft.AspNetCore.Http;
using SFA.DAS.HashingService;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.RouteValues;

namespace SFA.DAS.ProviderRelationships.Web.Authorization
{
    public class AuthorizationContextProvider : IAuthorizationContextProvider
    {
        private readonly HttpContext _httpContext;
        private readonly IHashingService _hashingService;
        private readonly IAuthenticationService _authenticationService;

        public AuthorizationContextProvider(HttpContext httpContext, IHashingService hashingService, IAuthenticationService authenticationService)
        {
            _httpContext = httpContext;
            _hashingService = hashingService;
            _authenticationService = authenticationService;
        }

        public IAuthorizationContext GetAuthorizationContext()
        {
            var authorizationContext = new AuthorizationContext();
            var accountValues = GetAccountValues();
            var userValues = GetUserValues();
            
            //todo
            //authorizationContext.AddEmployerFeatureValues(accountValues.Id, userValues.Email);
            //authorizationContext.AddEmployerUserRoleValues(accountValues.Id, userValues.Ref);
            
            return authorizationContext;
        }

        private (string HashedId, long? Id) GetAccountValues()
        {
            if (!_httpContext.Request.RouteValues.TryGetValue(RouteValueKeys.AccountHashedId, out var accountHashedId))
            {
                return (null, null);
            }
            
            if (!_hashingService.TryDecodeValue(accountHashedId.ToString(), out var accountId))
            {
                throw new UnauthorizedAccessException();
            }

            return (accountHashedId.ToString(), accountId);
        }

        private (Guid? Ref, string Email) GetUserValues()
        {
            if (!_authenticationService.IsUserAuthenticated())
            {
                return (null, null);
            }

            if (!_authenticationService.TryGetCurrentUserClaimValue(EmployerClaimTypes.UserId, out var userRefClaimValue))
            {
                throw new UnauthorizedAccessException();
            }

            if (!Guid.TryParse(userRefClaimValue, out var userRef))
            {
                throw new UnauthorizedAccessException();
            }

            if (!_authenticationService.TryGetCurrentUserClaimValue(EmployerClaimTypes.EmailAddress, out var userEmail))
            {
                throw new UnauthorizedAccessException();
            }

            return (userRef, userEmail);
        }
    }
}