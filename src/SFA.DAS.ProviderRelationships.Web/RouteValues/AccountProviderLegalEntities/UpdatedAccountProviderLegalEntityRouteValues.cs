using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities
{
    public class UpdatedAccountProviderLegalEntityRouteValues : IAuthorizationContextModel
    {
        [Required]
        public long? AccountId { get; set; }

        [Required]
        public long? AccountProviderLegalEntityId { get; set; }
    }
}