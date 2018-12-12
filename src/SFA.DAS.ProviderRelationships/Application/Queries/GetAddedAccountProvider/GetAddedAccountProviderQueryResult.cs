using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider
{
    public class GetAddedAccountProviderQueryResult
    {
        public AccountProviderDto AccountProvider { get; }

        public GetAddedAccountProviderQueryResult(AccountProviderDto accountProvider)
        {
            AccountProvider = accountProvider;
        }
    }
}