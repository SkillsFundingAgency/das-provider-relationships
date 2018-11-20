using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderUkprnsByAccountIdQuery : IRequest<GetAccountProviderUkprnsByAccountIdQueryResult>
    {
        public long AccountId { get; }

        public GetAccountProviderUkprnsByAccountIdQuery(long accountId)
        {
            AccountId = accountId;
        }
    }
}