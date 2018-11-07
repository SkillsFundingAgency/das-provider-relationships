using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAddedAccountProviderQueryResult
    {
        public AddedAccountProviderDto AddedAccountProvider { get; }

        public GetAddedAccountProviderQueryResult(AddedAccountProviderDto addedAccountProvider)
        {
            AddedAccountProvider = addedAccountProvider;
        }
    }
}