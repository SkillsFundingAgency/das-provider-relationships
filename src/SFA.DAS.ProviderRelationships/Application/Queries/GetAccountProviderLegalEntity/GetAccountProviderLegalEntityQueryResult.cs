using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity.Dtos;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity
{
    public class GetAccountProviderLegalEntityQueryResult
    {
        public AccountProviderBasicDto AccountProvider { get; }
        public AccountLegalEntityDto AccountLegalEntity { get; }
        public AccountProviderLegalEntitySummaryDto AccountProviderLegalEntitySummary { get; }
        public int AccountLegalEntitiesCount { get; }

        public GetAccountProviderLegalEntityQueryResult(
            AccountProviderBasicDto accountProvider,
            AccountLegalEntityDto accountLegalEntity,
            AccountProviderLegalEntitySummaryDto providerLegalEntitySummary,
            int accountLegalEntitiesCount)
        {
            AccountProvider = accountProvider;
            AccountLegalEntity = accountLegalEntity;
            AccountProviderLegalEntitySummary = providerLegalEntitySummary;
            AccountLegalEntitiesCount = accountLegalEntitiesCount;
        }
    }
}