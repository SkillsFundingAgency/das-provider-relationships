namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders
{
    public class AddedAccountProviderRouteValues
    {
        [Required]
        public long? AccountId { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }
    }
}