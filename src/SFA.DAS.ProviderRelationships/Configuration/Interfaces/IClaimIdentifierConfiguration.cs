namespace SFA.DAS.ProviderRelationships.Configuration.Interfaces
{
    public interface IClaimIdentifierConfiguration
    {
        string ClaimsBaseUrl { get; }
        string Id { get; }
        string Email { get; }
        string DisplayName { get; }
    }
}
