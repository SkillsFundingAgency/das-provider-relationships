using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetUpdatedAccountProviderLegalEntityQueryResult
    {
        public AccountProviderLegalEntityBasicDto AccountProviderLegalEntity { get; }
        public int AccountLegalEntityCount { get; }

        public GetUpdatedAccountProviderLegalEntityQueryResult(AccountProviderLegalEntityBasicDto accountProviderLegalEntity, int accountLegalEntityCount)
        {
            AccountProviderLegalEntity = accountProviderLegalEntity;
            AccountLegalEntityCount = accountLegalEntityCount;
        }
    }
}