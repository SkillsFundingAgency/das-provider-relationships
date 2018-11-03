using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class SearchProvidersQuery : IRequest<SearchProvidersQueryResponse>
    {
        public string Ukprn { get; }

        public SearchProvidersQuery(string ukprn)
        {
            Ukprn = ukprn;
        }
    }
}