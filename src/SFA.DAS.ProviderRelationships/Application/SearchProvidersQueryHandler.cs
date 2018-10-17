using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Validation;
using SFA.DAS.Providers.Api.Client;
using SFA.DAS.Validation;

namespace SFA.DAS.ProviderRelationships.Application
{
    public class SearchProvidersQueryHandler : IRequestHandler<SearchProvidersQuery, SearchProvidersQueryResponse>
    {
        private readonly IProviderApiClient _providerApiClient;

        public SearchProvidersQueryHandler(IProviderApiClient providerApiClient)
        {
            _providerApiClient = providerApiClient;
        }

        public async Task<SearchProvidersQueryResponse> Handle(SearchProvidersQuery request, CancellationToken cancellationToken)
        {
            var isUkprnValid = await _providerApiClient.ExistsAsync(request.Ukprn);

            if (!isUkprnValid)
            {
                throw new ValidationException().AddError(request, r => r.Ukprn, ErrorMessages.InvalidUkprn);
            }

            return new SearchProvidersQueryResponse
            {
                Ukprn = int.Parse(request.Ukprn)
            };
        }
    }
}