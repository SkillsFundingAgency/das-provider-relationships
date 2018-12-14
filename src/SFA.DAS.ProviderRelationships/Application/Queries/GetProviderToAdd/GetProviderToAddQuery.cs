using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd
{
    public class GetProviderToAddQuery : IRequest<GetProviderToAddQueryResult>
    {
        public long Ukprn { get; }

        public GetProviderToAddQuery(long ukprn)
        {
            Ukprn = ukprn;
        }
    }
}