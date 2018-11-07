using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class AlreadyAddedProviderViewModel
    {
        public AccountProviderDto AccountProvider { get; set; }

        [Required]
        public int? AccountProviderId { get; set; }

        [Required(ErrorMessage = "Option required")]
        [RegularExpression("SetPermissions|AddTrainingProvider", ErrorMessage = "Option required")]
        public string Choice { get; set; }
    }
}