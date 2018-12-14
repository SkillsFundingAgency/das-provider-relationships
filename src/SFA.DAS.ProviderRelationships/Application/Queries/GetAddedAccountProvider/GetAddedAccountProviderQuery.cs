using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider
{
    public class GetAddedAccountProviderQuery : IRequest<GetAddedAccountProviderQueryResult>
    {
        public long AccountId { get; }
        public long AccountProviderId { get; }

        public GetAddedAccountProviderQuery(long accountId, long accountProviderId)
        {
            AccountId = accountId;
            AccountProviderId = accountProviderId;
        }
    }
}