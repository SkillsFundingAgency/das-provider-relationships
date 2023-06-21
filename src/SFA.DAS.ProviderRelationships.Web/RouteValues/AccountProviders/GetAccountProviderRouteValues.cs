namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders;

public class GetAccountProviderRouteValues
{
    [Required]
    public string AccountHashedId { get; set; }

    [Required]
    public long? AccountProviderId { get; set; }
}