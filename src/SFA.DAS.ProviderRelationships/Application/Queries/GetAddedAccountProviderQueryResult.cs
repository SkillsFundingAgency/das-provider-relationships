using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAddedAccountProviderQueryResult
    {
        public AddedAccountProviderDto AccountProvider { get; }

        public GetAddedAccountProviderQueryResult(AddedAccountProviderDto accountProvider)
        {
            AccountProvider = accountProvider;
        }
    }
}