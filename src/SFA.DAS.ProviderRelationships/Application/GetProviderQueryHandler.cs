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
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderQueryResponse>
    {
        private readonly IProviderApiClient _providerApiClient;
        private readonly IMapper _mapper;

        public GetProviderQueryHandler(IProviderApiClient providerApiClient, IMapper mapper)
        {
            _providerApiClient = providerApiClient;
            _mapper = mapper;
        }

        public async Task<GetProviderQueryResponse> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            var providerResponse = await _providerApiClient.TryGetAsync(request.Ukprn);

            if (providerResponse == null)
            {
                throw new ValidationException().AddError(request, r => r.Ukprn, ErrorMessages.InvalidUkprn);
            }

            var provider = _mapper.Map<ProviderDto>(providerResponse);

            return new GetProviderQueryResponse
            {
                Provider = provider
            };
        }
    }
}