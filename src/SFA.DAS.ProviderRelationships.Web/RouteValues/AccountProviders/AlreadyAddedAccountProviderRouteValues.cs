using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders
{
    public class AlreadyAddedAccountProviderRouteValues
    {
        [Required]
        public long? AccountId { get; set; }
        
        [Required]
        public long? AccountProviderId { get; set; }
    }
}