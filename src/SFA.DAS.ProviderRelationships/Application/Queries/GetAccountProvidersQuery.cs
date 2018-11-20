using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProvidersQuery : IRequest<GetAccountProvidersQueryResult>
    {
        public long AccountId { get; }

        public GetAccountProvidersQuery(long accountId)
        {
            AccountId = accountId;
        }
    }
}