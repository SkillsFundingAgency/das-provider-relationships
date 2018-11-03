using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class AddedProviderViewModel
    {
        public AccountProviderDto AccountProvider { get; set; }

        [Required]
        public int? AccountProviderId { get; set; }

        [Required(ErrorMessage = "Option required")]
        [RegularExpression("SetPermissions|AddTrainingProvider|GoToHomepage", ErrorMessage = "Option required")]
        public string Choice { get; set; }
    }
}