using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider
{
    public class GetAccountProviderQueryResult
    {
        public AccountProviderDto AccountProvider { get; }
        public bool IsUpdatePermissionsOperationAuthorized { get; }

        public GetAccountProviderQueryResult(AccountProviderDto accountProvider, bool isOwner)
        {
            AccountProvider = accountProvider;
            IsUpdatePermissionsOperationAuthorized = isOwner;
        }
    }
}