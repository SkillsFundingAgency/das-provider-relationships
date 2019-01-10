using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity
{
    public class GetAccountProviderLegalEntityQueryResult
    {
        public AccountProviderDto AccountProvider { get; }
        public AccountLegalEntityDto AccountLegalEntity { get; }
        public AccountProviderLegalEntityDto AccountProviderLegalEntity { get; }
        public int AccountLegalEntitiesCount { get; }

        public GetAccountProviderLegalEntityQueryResult(
            AccountProviderDto accountProvider,
            AccountLegalEntityDto accountLegalEntity,
            AccountProviderLegalEntityDto providerLegalEntity,
            int accountLegalEntitiesCount)
        {
            AccountProvider = accountProvider;
            AccountLegalEntity = accountLegalEntity;
            AccountProviderLegalEntity = providerLegalEntity;
            AccountLegalEntitiesCount = accountLegalEntitiesCount;
        }
    }
}