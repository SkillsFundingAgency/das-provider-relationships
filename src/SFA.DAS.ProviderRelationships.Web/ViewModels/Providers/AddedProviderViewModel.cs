using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Providers
{
    public class AddedProviderViewModel
    {
        public AddedAccountProviderDto AccountProvider { get; set; }

        [Required]
        public int? AccountProviderId { get; set; }

        [Required(ErrorMessage = "Option required")]
        [RegularExpression("SetPermissions|AddTrainingProvider|GoToHomepage", ErrorMessage = "Option required")]
        public string Choice { get; set; }
    }
}