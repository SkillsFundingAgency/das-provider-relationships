using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.Providers.Api.Client;

namespace SFA.DAS.ProviderRelationships.Application.Queries
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
                return null;
            }

            var provider = _mapper.Map<ProviderDto>(providerResponse);

            return new GetProviderQueryResponse
            {
                Provider = provider
            };
        }
    }
}