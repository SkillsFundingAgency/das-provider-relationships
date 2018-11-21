using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities
{
    public class GetAccountProviderLegalEntityViewModel : IAuthorizationContextModel
    {
        public AccountProviderBasicDto AccountProvider { get; set; }
        public AccountLegalEntityBasicDto AccountLegalEntity { get; set; }
        
        [Required]
        public List<OperationViewModel> Operations { get; set; }
        
        [Required]
        public long? AccountId { get; set; }
        
        [Required]
        public long? AccountProviderId { get; set; }

        [Required]
        public long? AccountLegalEntityId { get; set; }
    }
}