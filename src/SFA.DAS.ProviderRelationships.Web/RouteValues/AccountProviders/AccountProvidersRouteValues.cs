using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders
{
    public class AccountProvidersRouteValues
    {
        [Required]
        public long? AccountId { get; set; } 
    }
}