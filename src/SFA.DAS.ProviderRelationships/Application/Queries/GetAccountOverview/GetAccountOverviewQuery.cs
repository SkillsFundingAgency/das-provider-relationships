using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountOverview
{
    public class GetAccountOverviewQuery : IRequest<GetAccountOverviewQueryResult>
    {
        public long AccountId { get; }

        public GetAccountOverviewQuery(long accountId)
        {
            AccountId = accountId;
        }
    }
}