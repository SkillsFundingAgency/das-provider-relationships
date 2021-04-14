using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class GetAccountProviderViewModel
    {
        public string AccountHashedId { get; set; }
        public AccountProviderDto AccountProvider { get; set; }
        public bool IsUpdatePermissionsOperationAuthorized { get; set; }
    }
}