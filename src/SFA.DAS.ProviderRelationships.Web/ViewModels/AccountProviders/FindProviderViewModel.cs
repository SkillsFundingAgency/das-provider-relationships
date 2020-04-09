using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd.Dtos;
using SFA.DAS.ProviderRelationships.Validation;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class FindProviderViewModel : IAuthorizationContextModel
    {
        [Required]
        public long? AccountId { get; set; }

        [Required(ErrorMessage = ErrorMessages.RequiredUkprn)]
        [RegularExpression(@"[\d+]{8}", ErrorMessage = ErrorMessages.InvalidUkprn)]
        public string Ukprn { get; set; }

        public List<ProviderDto> ProviderDto { get; set; }
    }
}