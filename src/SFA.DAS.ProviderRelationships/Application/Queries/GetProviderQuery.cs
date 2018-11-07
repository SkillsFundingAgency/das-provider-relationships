using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetProviderQuery : IRequest<GetProviderQueryReply>
    {
        public long Ukprn { get;  }

        public GetProviderQuery(long ukprn)
        {
            Ukprn = ukprn;
        }
    }
}