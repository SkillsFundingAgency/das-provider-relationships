using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderUkprnsQuery : IRequest<GetAccountProviderUkprnsQueryResult>
    {
        public long AccountId { get; }

        public GetAccountProviderUkprnsQuery(long accountId)
        {
            AccountId = accountId;
        }
    }
}