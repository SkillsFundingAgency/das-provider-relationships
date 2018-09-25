
namespace SFA.DAS.ProviderRelationships.Authentication.Interfaces
{
    public interface IClaimValue
    {
        string DisplayName { get; }
        string Email { get; }
        string Id { get; }
    }
}
