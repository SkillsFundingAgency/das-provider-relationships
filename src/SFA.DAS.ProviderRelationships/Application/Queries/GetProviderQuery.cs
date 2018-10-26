using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetProviderQuery : IRequest<GetProviderQueryResponse>
    {
        public long Ukprn { get;  }

        public GetProviderQuery(long ukprn)
        {
            Ukprn = ukprn;
        }
    }
}