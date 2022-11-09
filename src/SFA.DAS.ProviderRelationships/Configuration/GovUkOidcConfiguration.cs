namespace SFA.DAS.ProviderRelationships.Configuration;

public class GovSignIn
{
    public GovUkOidcConfiguration GovUkOidcConfiguration { get; set; }
}

public class GovUkOidcConfiguration
{
    public string BaseUrl { get; set; }
    public string ClientId { get; set; }
    public string KeyVaultIdentifier { get; set; }
}