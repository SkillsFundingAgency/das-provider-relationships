using SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity
{
    public class GetUpdatedAccountProviderLegalEntityQueryResult
    {
        public AccountProviderLegalEntityDto AccountProviderLegalEntity { get; }
        public int AccountLegalEntitiesCount { get; }

        public GetUpdatedAccountProviderLegalEntityQueryResult(AccountProviderLegalEntityDto accountProviderLegalEntity, int accountLegalEntitiesCount)
        {
            AccountProviderLegalEntity = accountProviderLegalEntity;
            AccountLegalEntitiesCount = accountLegalEntitiesCount;
        }
    }
}