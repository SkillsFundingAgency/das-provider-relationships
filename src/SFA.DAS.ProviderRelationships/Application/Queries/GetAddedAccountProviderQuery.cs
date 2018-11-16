using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAddedAccountProviderQuery : IRequest<GetAddedAccountProviderQueryResult>
    {
        public long AccountId { get;  }
        public int AccountProviderId { get; }

        public GetAddedAccountProviderQuery(long accountId, int accountProviderId)
        {
            AccountId = accountId;
            AccountProviderId = accountProviderId;
        }
    }
}