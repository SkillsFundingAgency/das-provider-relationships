namespace SFA.DAS.ProviderRelationships.Configuration;

public interface IProviderRelationshipsConfiguration
{
    string CdnBaseUrl { get; set; }
    string AllowedHashstringCharacters { get; set; }
    string Hashstring { get; set; }
    string DatabaseConnectionString { get; set; }
    string ServiceBusConnectionString { get; set; }
    string NServiceBusLicense { get; set; }
    string ZenDeskSnippetKey { get; set; }
    string ZenDeskSectionId { get; set; }
    string ApprenticeshipProgrammesApiBaseUrl { get; set; }
    string ProviderPortalBaseUrl { get; set; }
    string EnvironmentName { get; set; }
    string ApplicationBaseUrl { get; set; }
    bool UseGovUkSignIn { get; set; }
}