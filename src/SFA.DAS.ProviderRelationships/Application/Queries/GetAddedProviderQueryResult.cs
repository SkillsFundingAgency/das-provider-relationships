using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAddedProviderQueryResult
    {
        public AccountProviderDto AccountProvider { get; }

        public GetAddedProviderQueryResult(AccountProviderDto accountProvider)
        {
            AccountProvider = accountProvider;
        }
    }
}