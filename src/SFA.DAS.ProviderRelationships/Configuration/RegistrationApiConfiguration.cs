using SFA.DAS.Http.Configuration;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class RegistrationApiConfiguration : IRegistrationApiConfiguration
    {
        public string BaseUrl { get; set; }
        public string IdentifierUri { get; set; }
    }
}
