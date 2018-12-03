namespace SFA.DAS.ProviderRelationships.Authentication.Oidc
{
    public sealed class AuthenticationUrls : IAuthenticationUrls
    {
        public string AuthorizeEndpoint => GetEndpoint(_configuration.AuthorizeEndpoint);
        public string LogoutEndpoint => GetEndpoint(_configuration.LogoutEndpoint, _authenticationService.GetCurrentUserClaimValue("id_token"));
        public string TokenEndpoint => GetEndpoint(_configuration.TokenEndpoint);
        public string UserInfoEndpoint => GetEndpoint(_configuration.UserInfoEndpoint);
        
        private readonly IOidcConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationUrls(IOidcConfiguration configuration, IAuthenticationService authenticationService)
        {
            _configuration = configuration;
            _authenticationService = authenticationService;
        }

        private string GetEndpoint(string endpoint, params object[] args)
        {
            return $"{_configuration.BaseAddress}{string.Format(endpoint, args)}";
        }
    }
}