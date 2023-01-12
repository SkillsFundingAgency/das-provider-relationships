using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.ProviderRelationships.Validation;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class FindProviderEditModel : IAuthorizationContextModel
    {
        [Required]
        public long? AccountId { get; set; }

        [Required(ErrorMessage = ErrorMessages.RequiredUkprn)]
        public string Ukprn { get; set; }
    }
}