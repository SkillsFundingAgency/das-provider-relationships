using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd
{
    public class FindProviderToAddQuery : IRequest<FindProviderToAddQueryResult>
    {
        public long AccountId { get; }
        public long Ukprn { get; }

        public FindProviderToAddQuery(long accountId, long ukprn)
        {
            AccountId = accountId;
            Ukprn = ukprn;
        }
    }
}