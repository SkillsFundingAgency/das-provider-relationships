using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetUpdatedAccountProviderLegalEntityQueryResult
    {
        public AccountProviderLegalEntityBasicDto AccountProviderLegalEntity { get; }
        public int AccountLegalEntitiesCount { get; }

        public GetUpdatedAccountProviderLegalEntityQueryResult(AccountProviderLegalEntityBasicDto accountProviderLegalEntity, int accountLegalEntitiesCount)
        {
            AccountProviderLegalEntity = accountProviderLegalEntity;
            AccountLegalEntitiesCount = accountLegalEntitiesCount;
        }
    }
}