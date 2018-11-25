using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities
{
    public class UpdatedAccountProviderLegalEntityViewModel
    {
        [Required(ErrorMessage = "Option required")]
        [RegularExpression("SetPermissions|GoToHomepage|AddTrainingProvider", ErrorMessage = "Option required")]
        public string Choice { get; set; }
    }
}