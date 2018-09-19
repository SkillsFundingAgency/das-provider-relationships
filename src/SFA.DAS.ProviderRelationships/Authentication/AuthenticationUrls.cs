using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    public sealed class AuthenticationUrls
    {
        private readonly IdentityServerConfiguration _config;

        public AuthenticationUrls(IdentityServerConfiguration config)
        {
            _config = config;
        }

        public string AuthorizeEndpoint => Generate(_config.AuthorizeEndPoint);
        public string TokenEndpoint => Generate(_config.TokenEndpoint);
        public string UserInfoEndpoint => Generate(_config.UserInfoEndpoint);

        //public string LogoutEndpoint() => $"{_configuration.BaseAddress}{_configuration.LogoutEndpoint}";
        //public string RegisterLink() => _configuration.BaseAddress.Replace("/identity", "") + string.Format(_configuration.RegisterLink, _configuration.ClientId);
        //public string RequiresVerification() => _baseUrl + "requires_verification";

        private string Generate(string endpoint)
        {
            return $"{_config.BaseAddress}{endpoint}";
        }
    }
}
