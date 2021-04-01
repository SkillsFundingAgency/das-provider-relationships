using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Operations
{
    public class ConfirmOperationViewModel : IAuthorizationContextModel
    {
        [Required]
        public long? AccountProviderId { get; set; }

        [Required]
        public long? AccountLegalEntityId { get; set; }

        public string AccountLegalEntityName { get; set; }
        public string ProviderName { get; set; }

        [Required]
        public List<OperationViewModel> Operations { get; set; }

        public AccountProviderDto AccountProvider { get; set; }
        public AccountLegalEntityDto AccountLegalEntity { get; set; }

        public string BackLink { get; set; }
    }
}