using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Providers
{
    public class AddProviderRouteValues
    {
        [Required]
        public long? Ukprn { get; set; }
    }
}