using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity.Dtos;
using SFA.DAS.ProviderRelationships.DtosShared;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities
{
    public class UpdateAccountProviderLegalEntityViewModel : IAuthorizationContextModel
    {
        public AccountProviderBasicDto AccountProvider { get; set; }
        public AccountLegalEntityBasicDto AccountLegalEntity { get; set; }

        [Required]
        public long? AccountId { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }

        [Required]
        public long? AccountLegalEntityId { get; set; }

        [Required]
        public Guid? UserRef { get; set; }
        
        [Required]
        public List<OperationViewModel> Operations { get; set; }
    }
}