using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities
{
    public class GetAccountProviderLegalEntityViewModel
    {
        public AccountProviderBasicDto AccountProvider { get; set; }
        public AccountLegalEntityBasicDto AccountLegalEntity { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }

        [Required]
        public long? AccountLegalEntityId { get; set; }

        [Required]
        public List<OperationViewModel> Operations { get; set; }
    }
}