using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider
{
    public class GetAddedAccountProviderQueryResult
    {
        public AccountProviderBasicDto AccountProvider { get; }

        public GetAddedAccountProviderQueryResult(AccountProviderBasicDto accountProvider)
        {
            AccountProvider = accountProvider;
        }
    }
}