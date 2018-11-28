using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderQueryResult
    {
        public AccountProviderDto AccountProvider { get; }

        public GetAccountProviderQueryResult(AccountProviderDto accountProvider)
        {
            AccountProvider = accountProvider;
        }
    }
}