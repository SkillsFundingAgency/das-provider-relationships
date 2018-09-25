using SFA.DAS.ProviderRelationships.Authentication.Interfaces;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    public sealed class ClaimValue : IClaimValue
    {
        private readonly IClaimIdentifierConfiguration _config;

        //todo: extensions on ClaimIdentifierConfiguration?
        public ClaimValue(IClaimIdentifierConfiguration config)
        {
            _config = config;
        }

        //todo: generate once?
        public string DisplayName => Generate(_config.DisplayName);
        public string Email => Generate(_config.Email);
        public string Id => Generate(_config.Id);

        //public string FamilyName() => Generate(_config.FamilyName);
        //public string GivenName() => Generate(_config.GivenName);

        private string Generate(string claimType)
        {
            return $"{_config.ClaimsBaseUrl}{claimType}";
        }
    }
}
