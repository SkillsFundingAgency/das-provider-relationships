using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderLegalEntityQueryResult
    {
        public AccountProviderBasicDto AccountProvider { get; }
        public AccountLegalEntityBasicDto AccountLegalEntity { get; }
        public AccountProviderLegalEntityDto AccountProviderLegalEntity { get; }
        public int AccountLegalEntitiesCount { get; }

        public GetAccountProviderLegalEntityQueryResult(AccountProviderBasicDto accountProvider, AccountLegalEntityBasicDto accountLegalEntity, AccountProviderLegalEntityDto accountProviderLegalEntity, int accountLegalEntitiesCount)
        {
            AccountProvider = accountProvider;
            AccountLegalEntity = accountLegalEntity;
            AccountProviderLegalEntity = accountProviderLegalEntity;
            AccountLegalEntitiesCount = accountLegalEntitiesCount;
        }
    }
}