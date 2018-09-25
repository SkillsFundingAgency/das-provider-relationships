using MediatR;

namespace SFA.DAS.ProviderRelationships.Application
{
    public class GetProviderQuery : IRequest<GetProviderQueryResponse>
    {
        public string Ukprn { get; set; }
    }
}