using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Web.ViewModels.Permissions;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities
{
    public class AccountProviderLegalEntityViewModel
    {
        public AccountProviderLegalEntityViewModel()
        {
            Permissions = Enum.GetValues(typeof(Permission))
                .Cast<Permission>()
                .Select(p => new PermissionViewModel {Value = p}).ToList();
        }

        public AccountProviderDto AccountProvider { get; set; }
        public AccountLegalEntityDto AccountLegalEntity { get; set; }
        public int AccountLegalEntitiesCount { get; set; }

        [Required]
        public string AccountHashedId { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }

        [Required]
        public long? AccountLegalEntityId { get; set; }

        public Guid? UserRef { get; set; }

        [Required]
        public List<PermissionViewModel> Permissions { get; set; }

        public bool IsProviderBlockedFromRecruit { get; set; }
       
        public bool? Confirmation { get; set; }
    }
}