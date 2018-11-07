using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAddedProviderQueryReply
    {
        public AccountProviderDto AccountProvider { get; }

        public GetAddedProviderQueryReply(AccountProviderDto accountProvider)
        {
            AccountProvider = accountProvider;
        }
    }
}