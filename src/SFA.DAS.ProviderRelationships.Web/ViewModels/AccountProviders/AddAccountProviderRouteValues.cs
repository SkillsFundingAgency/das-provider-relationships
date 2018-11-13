using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class AddAccountProviderRouteValues
    {
        [Required]
        public long? Ukprn { get; set; }
    }
}