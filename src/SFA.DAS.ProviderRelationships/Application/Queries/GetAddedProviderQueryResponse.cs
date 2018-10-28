using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAddedProviderQueryResponse
    {
        public AccountProviderDto AccountProvider { get; }

        public GetAddedProviderQueryResponse(AccountProviderDto accountProvider)
        {
            AccountProvider = accountProvider;
        }
    }
}