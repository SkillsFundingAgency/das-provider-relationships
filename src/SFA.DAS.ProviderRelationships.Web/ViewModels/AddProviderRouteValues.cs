using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class AddProviderRouteValues
    {
        [Required]
        public long? Ukprn { get; set; }
    }
}