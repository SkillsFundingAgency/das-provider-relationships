using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd.Dtos;
using SFA.DAS.ProviderRelationships.Validation;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class FindProviderViewModel
    {
        [Required]
        public long? AccountId { get; set; }

        [Required(ErrorMessage = ErrorMessages.RequiredUkprn)]
        public string Ukprn { get; set; }

        public List<ProviderDto> Providers { get; set; }
    }
}