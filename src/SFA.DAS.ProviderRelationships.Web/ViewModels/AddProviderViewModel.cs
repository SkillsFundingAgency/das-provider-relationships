using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class AddProviderViewModel
    {
        [Required]
        public AddAccountProviderCommand AddAccountProviderCommand { get; set; }

        [Required(ErrorMessage = "Option required")]
        [RegularExpression("Confirm|ReEnterUkprn", ErrorMessage = "Option required")]
        public string Choice { get; set; }

        public ProviderDto Provider { get; set; }
    }
}