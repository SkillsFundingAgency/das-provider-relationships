using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Validation;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Providers
{
    public class FindProvidersViewModel : IAuthorizationContextModel
    {
        [Required(ErrorMessage = ErrorMessages.InvalidUkprn)]
        [RegularExpression(@"[\d+]{8}", ErrorMessage = ErrorMessages.InvalidUkprn)]
        public string Ukprn { get; set; }
        public List<ProviderDto> Providers { get; set; }
    }
}