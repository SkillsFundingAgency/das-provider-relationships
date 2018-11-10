using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class AlreadyAddedAccountProviderViewModel
    {
        public AddedAccountProviderDto AccountProvider { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }

        [Required(ErrorMessage = "Option required")]
        [RegularExpression("SetPermissions|AddTrainingProvider", ErrorMessage = "Option required")]
        public string Choice { get; set; }
    }
}