using SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class AddedAccountProviderViewModel
    {
        public string AccountHashedId { get; set; }
        public AccountProviderDto AccountProvider { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }

        [Required(ErrorMessage = "Option required")]
        [RegularExpression("SetPermissions|AddTrainingProvider|GoToHomepage", ErrorMessage = "Option required")]
        public string Choice { get; set; }
    }
}