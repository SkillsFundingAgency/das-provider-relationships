using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;
using SFA.DAS.ProviderRelationships.Validation;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Providers
{
    public class FindProvidersEditModel : IAuthorizationContextModel
    {
        [Required]
        public long? AccountId { get; set; }
        [Required(ErrorMessage = ErrorMessages.InvalidUkprn)]
        [RegularExpression(@"[\d+]{8}", ErrorMessage = ErrorMessages.InvalidUkprn)]
        public string ProviderUkprn { get; set; }
    }
}