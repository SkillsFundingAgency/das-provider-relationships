using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetProviderQuery : IRequest<GetProviderQueryResult>
    {
        public long Ukprn { get;  }

        public GetProviderQuery(long ukprn)
        {
            Ukprn = ukprn;
        }
    }
}