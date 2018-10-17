using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetProviderQuery : IRequest<GetProviderQueryResponse>
    {
        public string Ukprn { get; set; }
    }
}