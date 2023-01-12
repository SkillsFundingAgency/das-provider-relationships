using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class GetAccountProviderViewModel
    {
        public AccountProviderDto AccountProvider { get; set; }
        public bool IsUpdatePermissionsOperationAuthorized { get; set; }
    }
}