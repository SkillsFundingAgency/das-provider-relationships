namespace SFA.DAS.ProviderRelationships.Authentication
{
    public sealed class AuthenticationUrls : IAuthenticationUrls
    {
        public string AuthorizeEndpoint => Generate(_config.AuthorizeEndPoint);
        public string TokenEndpoint => Generate(_config.TokenEndpoint);
        public string UserInfoEndpoint => Generate(_config.UserInfoEndpoint);
        
        private readonly IIdentityServerConfiguration _config;

        public AuthenticationUrls(IIdentityServerConfiguration config)
        {
            _config = config;
        }

        private string Generate(string endpoint)
        {
            return $"{_config.BaseAddress}{endpoint}";
        }
    }
}