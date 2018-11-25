using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders
{
    public class AddAccountProviderRouteValues
    {
        [Required]
        public long? Ukprn { get; set; }
    }
}