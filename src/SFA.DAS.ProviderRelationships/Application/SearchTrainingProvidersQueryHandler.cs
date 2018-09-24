using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.Providers.Api.Client;
using SFA.DAS.Validation;

namespace SFA.DAS.ProviderRelationships.Application
{
    public class SearchTrainingProvidersQueryHandler : IRequestHandler<SearchTrainingProvidersQuery, SearchTrainingProvidersQueryResponse>
    {
        private readonly IProviderApiClient _providerApiClient;
        private readonly IMapper _mapper;

        public SearchTrainingProvidersQueryHandler(IProviderApiClient providerApiClient, IMapper mapper)
        {
            _providerApiClient = providerApiClient;
            _mapper = mapper;
        }

        public async Task<SearchTrainingProvidersQueryResponse> Handle(SearchTrainingProvidersQuery request, CancellationToken cancellationToken)
        {
            var trainingProviderResponse = await _providerApiClient.TryGetAsync(request.Ukprn);

            if (trainingProviderResponse == null)
            {
                throw new ValidationException().AddError(request, r => r.Ukprn, ErrorMessages.InvalidUkprn);
            }

            var trainingProvider = _mapper.Map<TrainingProviderDto>(trainingProviderResponse);

            return new SearchTrainingProvidersQueryResponse
            {
                TrainingProvider = trainingProvider
            };
        }
    }
}