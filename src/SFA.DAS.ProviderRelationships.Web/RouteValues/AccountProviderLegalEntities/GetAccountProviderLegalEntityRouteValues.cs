using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities
{
    public class GetAccountProviderLegalEntityRouteValues : IAuthorizationContextModel
    {
        [Required]
        public string AccountHashedId { get; set; }

        [Required]
        public long? AccountId { get; set; }
        
        [Required]
        public long? AccountProviderId { get; set; }

        [Required]
        public long? AccountLegalEntityId { get; set; }
    }
}