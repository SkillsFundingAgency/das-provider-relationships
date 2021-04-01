using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class ProviderRelationshipsConfiguration
    {
        public AzureActiveDirectoryConfiguration AzureActiveDirectory { get; set; }
        public OidcConfiguration Oidc { get; set; }
        public ReadStoreConfiguration ReadStore { get; set; }
        public EmployerUrlsConfiguration EmployerUrls { get; set; }
        public PasAccountApiConfiguration PasAccountApi { get; set; }
        public RegistrationApiConfiguration RegistrationApiClientConfiguration { get; set; }
        public string CdnBaseUrl { get; set; }

        public string AllowedHashstringCharacters { get; set; }
        public string Hashstring { get; set; }
        public string DatabaseConnectionString { get; set; }
        public string ServiceBusConnectionString { get; set; }

        public string NServiceBusLicense {
            get => _decodedNServiceBusLicense ?? (_decodedNServiceBusLicense = _nServiceBusLicense.HtmlDecode());
            set => _nServiceBusLicense = value;
        }
        
        private string _nServiceBusLicense;
        private string _decodedNServiceBusLicense;
        public string ZenDeskSnippetKey { get; set; }
        public string ZenDeskSectionId { get; set; }
        public string ApprenticeshipProgrammesApiBaseUrl { get; set; }
    }
}   