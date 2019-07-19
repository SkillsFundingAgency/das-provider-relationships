using MediatR;
using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.Client.Http
{
    public class ProviderRelationshipsApiClientFactory : IProviderRelationshipsApiClientFactory
    {
        private readonly AzureActiveDirectoryClientConfiguration _configuration;
        private readonly IMediator _mediator;

        public ProviderRelationshipsApiClientFactory(AzureActiveDirectoryClientConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        public IProviderRelationshipsApiClient CreateApiClient()
        {
            var httpClientFactory = new AzureActiveDirectoryHttpClientFactory(_configuration);
            var httpClient = httpClientFactory.CreateHttpClient();
            var restHttpClient = new RestHttpClient(httpClient);
            var apiClient = new ProviderRelationshipsApiClient(restHttpClient, _mediator);

            return apiClient;
        }
    }
}