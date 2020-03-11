using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviders
{
    public class GetAccountProvidersQueryResult
    {
        public List<AccountProviderDto> AccountProviders { get; }
        public int AccountLegalEntitiesCount { get; }
        public bool IsAddProviderOperationAuthorized { get; }
        public bool IsUpdatePermissionsOperationAuthorized { get; }

        public GetAccountProvidersQueryResult(List<AccountProviderDto> accountProviders, int accountLegalEntitiesCount, bool isOwner)
        {
            AccountProviders = accountProviders;
            AccountLegalEntitiesCount = accountLegalEntitiesCount;
            IsAddProviderOperationAuthorized = IsUpdatePermissionsOperationAuthorized = isOwner;
        }
    }
}