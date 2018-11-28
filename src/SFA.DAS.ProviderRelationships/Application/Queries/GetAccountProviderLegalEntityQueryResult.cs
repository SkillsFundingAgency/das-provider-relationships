using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderLegalEntityQueryResult
    {
        public AccountProviderBasicDto AccountProvider { get; }
        public AccountLegalEntityBasicDto AccountLegalEntity { get; }
        public AccountProviderLegalEntitySummaryDto AccountProviderLegalEntitySummary { get; }

        public GetAccountProviderLegalEntityQueryResult(AccountProviderBasicDto accountProvider, AccountLegalEntityBasicDto accountLegalEntity, AccountProviderLegalEntitySummaryDto providerLegalEntitySummary)
        {
            AccountProvider = accountProvider;
            AccountLegalEntity = accountLegalEntity;
            AccountProviderLegalEntitySummary = providerLegalEntitySummary;
        }
    }
}