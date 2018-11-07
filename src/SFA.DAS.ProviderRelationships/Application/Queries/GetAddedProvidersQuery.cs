using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAddedProvidersQuery : IRequest<GetAddedProvidersQueryReply>
    {
        public long AccountId { get; }

        public GetAddedProvidersQuery(long accountId)
        {
            AccountId = accountId;
        }
    }
}