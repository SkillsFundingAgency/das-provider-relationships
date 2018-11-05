using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAddedProviderQuery : IRequest<GetAddedProviderQueryResponse>
    {
        public long AccountId { get;  }
        public int AccountProviderId { get; }

        public GetAddedProviderQuery(long accountId, int accountProviderId)
        {
            AccountId = accountId;
            AccountProviderId = accountProviderId;
        }
    }
}