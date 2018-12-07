using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.DtosShared;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class AlreadyAddedAccountProviderViewModel
    {
        public AccountProviderBasicDto AccountProvider { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }

        [Required(ErrorMessage = "Option required")]
        [RegularExpression("SetPermissions|AddTrainingProvider", ErrorMessage = "Option required")]
        public string Choice { get; set; }
    }
}