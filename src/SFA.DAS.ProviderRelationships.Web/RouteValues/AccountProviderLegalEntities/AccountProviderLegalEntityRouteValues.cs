namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities
{
    public class AccountProviderLegalEntityRouteValues
    {
        [Required]
        public string AccountHashedId { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }

        [Required]
        public long? AccountLegalEntityId { get; set; }
    }
}