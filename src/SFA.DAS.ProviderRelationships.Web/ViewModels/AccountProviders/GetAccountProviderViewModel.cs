using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class GetAccountProviderViewModel
    {
        public AccountProviderDto AccountProvider { get; set; }
        public bool IsUpdatePermissionsOperationAuthorized { get; set; }
    }
}