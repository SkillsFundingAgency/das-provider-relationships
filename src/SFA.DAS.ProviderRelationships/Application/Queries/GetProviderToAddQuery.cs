using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetProviderToAddQuery : IRequest<GetProviderToAddQueryResult>
    {
        public long Ukprn { get;  }

        public GetProviderToAddQuery(long ukprn)
        {
            Ukprn = ukprn;
        }
    }
}