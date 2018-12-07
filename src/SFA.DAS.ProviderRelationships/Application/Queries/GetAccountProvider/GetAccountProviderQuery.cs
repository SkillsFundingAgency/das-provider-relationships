using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider
{
    public class GetAccountProviderQuery : IRequest<GetAccountProviderQueryResult>
    {
        public long AccountId { get;  }
        public long AccountProviderId { get; }

        public GetAccountProviderQuery(long accountId, long accountProviderId)
        {
            AccountId = accountId;
            AccountProviderId = accountProviderId;
        }
    }
}