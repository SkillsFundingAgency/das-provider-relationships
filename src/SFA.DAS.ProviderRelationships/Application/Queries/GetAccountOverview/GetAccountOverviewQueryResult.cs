namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountOverview
{
    public class GetAccountOverviewQueryResult
    {
        public int AccountProvidersCount { get; }
        public int AccountLegalEntitiesCount { get; }
        public bool IsAddProviderOperationAuthorized { get; }
        public bool IsUpdatePermissionsOperationAuthorized { get; }

        public GetAccountOverviewQueryResult(int accountProvidersCount, int accountLegalEntitiesCount, bool isOwner)
        {
            AccountProvidersCount = accountProvidersCount;
            AccountLegalEntitiesCount = accountLegalEntitiesCount;
            IsAddProviderOperationAuthorized = IsUpdatePermissionsOperationAuthorized = isOwner;
        }
    }
}