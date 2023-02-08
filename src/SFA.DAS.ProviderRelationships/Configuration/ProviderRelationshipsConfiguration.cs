using SFA.DAS.ProviderRelationships.Extensions;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class ProviderRelationshipsConfiguration
    {
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
        public string ProviderPortalBaseUrl { get; set; }
        public string EnvironmentName { get; set; }
        public string ApplicationBaseUrl { get; set; }
        public bool UseGovSignIn { get; set; }
    }
}   