using SFA.DAS.ProviderRelationships.Validation;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class FindProviderEditModel
    {
        [Required]
        public string AccountHashedId { get; set; }

        [Required(ErrorMessage = ErrorMessages.RequiredUkprn)]
        public string Ukprn { get; set; }
    }
}