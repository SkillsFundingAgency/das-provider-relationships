using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Authorization
{
    public sealed class AuthorizationUrls
    {
        private readonly string _baseUrl;
        private readonly IdentityServerConfiguration _configuration;

        public AuthorizationUrls(IdentityServerConfiguration configuration)
        {
            _baseUrl = configuration.ClaimIdentifierConfiguration.ClaimsBaseUrl;
            _configuration = configuration;
        }

        public string AuthorizeEndpoint() => $"{_configuration.BaseAddress}{_configuration.AuthorizeEndPoint}";
        public string DisplayName() => $"{_baseUrl}{_configuration.ClaimIdentifierConfiguration.DisplayName}";
        public string Email() => $"{_baseUrl}{_configuration.ClaimIdentifierConfiguration.Email}";
        //public string FamilyName() => _baseUrl + _configuration.ClaimIdentifierConfiguration.FamilyName;
        //public string GivenName() => _baseUrl + _configuration.ClaimIdentifierConfiguration.GivenName;
        public string Id() => $"{_baseUrl}{_configuration.ClaimIdentifierConfiguration.Id}";
        //public string LogoutEndpoint() => $"{_configuration.BaseAddress}{_configuration.LogoutEndpoint}";
        //public string RegisterLink() => _configuration.BaseAddress.Replace("/identity", "") + string.Format(_configuration.RegisterLink, _configuration.ClientId);
        //public string RequiresVerification() => _baseUrl + "requires_verification";
        public string TokenEndpoint() => $"{_configuration.BaseAddress}{_configuration.TokenEndpoint}";
        public string UserInfoEndpoint() => $"{_configuration.BaseAddress}{_configuration.UserInfoEndpoint}";
    }
}
