using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    public sealed class AuthenticationUrls : IAuthenticationUrls
    {
        private readonly IIdentityServerConfiguration _config;

        public AuthenticationUrls(IIdentityServerConfiguration config)
        {
            _config = config;
        }

        public string AuthorizeEndpoint => Generate(_config.AuthorizeEndPoint);
        public string TokenEndpoint => Generate(_config.TokenEndpoint);
        public string UserInfoEndpoint => Generate(_config.UserInfoEndpoint);

        public string ChangePasswordLink => GenerateChangeUrl(_config.ChangePasswordLink);
        public string ChangeEmailLink => GenerateChangeUrl(_config.ChangeEmailLink);

        //public string LogoutEndpoint() => $"{_configuration.BaseAddress}{_configuration.LogoutEndpoint}";
        //public string RegisterLink() => _configuration.BaseAddress.Replace("/identity", "") + string.Format(_configuration.RegisterLink, _configuration.ClientId);
        //public string RequiresVerification() => _baseUrl + "requires_verification";

        private string Generate(string endpoint)
        {
            return $"{_config.BaseAddress}{endpoint}";
        }

        private string GenerateChangeUrl(string pathFormat)
        {
            return $"{_config.BaseAddress.Replace("/identity", "")}{string.Format(pathFormat, _config.ClientId)}";
        }
    }
}
