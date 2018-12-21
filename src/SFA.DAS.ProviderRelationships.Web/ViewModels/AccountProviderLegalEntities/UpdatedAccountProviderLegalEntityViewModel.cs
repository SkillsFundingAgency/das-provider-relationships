using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities
{
    public class UpdatedAccountProviderLegalEntityViewModel
    {
        public AccountProviderLegalEntityDto AccountProviderLegalEntity { get; set; }
        public int AccountLegalEntitiesCount { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }
        
        [Required(ErrorMessage = "Option required")]
        [RegularExpression("SetPermissions|AddTrainingProvider|GoToHomepage", ErrorMessage = "Option required")]
        public string Choice { get; set; }
    }
}