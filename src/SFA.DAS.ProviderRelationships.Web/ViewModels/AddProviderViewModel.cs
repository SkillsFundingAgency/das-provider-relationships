using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class AddProviderViewModel
    {
        [Required(ErrorMessage = "Option required")]
        [RegularExpression("Confirm|ReEnterUkprn", ErrorMessage = "Option required")]
        public string Choice { get; set; }

        public ProviderDto Provider { get; set; }

        [Required]
        [RegularExpression(@"[\d+]{8}")]
        public string Ukprn { get; set; }
    }
}