using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity.Dtos;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities
{
    public class GetAccountProviderLegalEntityViewModel
    {
        public AccountProviderBasicDto AccountProvider { get; set; }
        public AccountLegalEntityDto AccountLegalEntity { get; set; }
        public int AccountLegalEntitiesCount { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }

        [Required]
        public long? AccountLegalEntityId { get; set; }

        [Required]
        public List<OperationViewModel> Operations { get; set; }
    }
}