using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class AccountProvidersViewModel
    {
        public string AccountHashedId { get; set; }
        public List<AccountProviderDto> AccountProviders { get; set; }
        public int AccountLegalEntitiesCount { get; set; }
        public bool IsAddProviderOperationAuthorized { get; set; }
        public bool IsUpdatePermissionsOperationAuthorized { get; set; }
    }
}