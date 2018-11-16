namespace SFA.DAS.ProviderRelationships.Authentication
{
    public sealed class AuthenticationUrls : IAuthenticationUrls
    {
        public string AuthorizeEndpoint => GetIdentityServerUrl(_configuration.AuthorizeEndpoint);
        public string ChangePasswordUrl => GetNonIdentityServerUrl(_configuration.ChangePasswordUrl, _configuration.ClientId);
        public string ChangeEmailUrl => GetNonIdentityServerUrl(_configuration.ChangeEmailUrl, _configuration.ClientId);
        public string LogoutEndpoint => GetIdentityServerUrl(_configuration.LogoutEndpoint, _authenticationService.GetCurrentUserClaimValue("id_token"));
        public string TokenEndpoint => GetIdentityServerUrl(_configuration.TokenEndpoint);
        public string UserInfoEndpoint => GetIdentityServerUrl(_configuration.UserInfoEndpoint);
        
        private readonly IIdentityServerConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationUrls(IIdentityServerConfiguration configuration, IAuthenticationService authenticationService)
        {
            _configuration = configuration;
            _authenticationService = authenticationService;
        }

        private string GetIdentityServerUrl(string endpoint, params object[] args)
        {
            return $"{_configuration.BaseAddress}{string.Format(endpoint, args)}";
        }

        private string GetNonIdentityServerUrl(string endpoint, params object[] args)
        {
            return $"{_configuration.BaseAddress.Replace("/identity", "")}{string.Format(endpoint, args)}";
        }
    }
}
