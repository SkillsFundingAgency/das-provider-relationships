namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders
{
    public class AddedAccountProviderRouteValues
    {
        public string AccountHashedId { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }
    }
}