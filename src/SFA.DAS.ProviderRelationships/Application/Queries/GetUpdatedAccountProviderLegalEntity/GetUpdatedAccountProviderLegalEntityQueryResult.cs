using SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity
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