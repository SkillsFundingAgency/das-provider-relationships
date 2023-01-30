using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities
{
    public class AccountProviderLegalEntityRouteValues
    {
        [Required]
        public long? AccountId { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }

        [Required]
        public long? AccountLegalEntityId { get; set; }
    }
}