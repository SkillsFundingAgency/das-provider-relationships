using System;
using System.Net.Http;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.Client.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        //private readonly IEnvironmentService _environmentService;
        private readonly ProviderRelationshipsApiClientConfiguration _configuration;

        public HttpClientFactory(//IEnvironmentService environmentService,
            ProviderRelationshipsApiClientConfiguration configuration)
        {
            //_environmentService = environmentService;
            _configuration = configuration;
        }

        public HttpClient CreateHttpClient()
        {
            var httpClientBuilder = new HttpClientBuilder()
                .WithDefaultHeaders()
                .WithBearerAuthorisationHeader(new AzureADBearerTokenGenerator(_configuration));

            //if (!_environmentService.IsCurrent(DasEnv.LOCAL))
//            httpClientBuilder.WithBearerAuthorisationHeader(new AzureADBearerTokenGenerator(_configuration));

            var httpClient = httpClientBuilder.Build();
            
            httpClient.BaseAddress = new Uri(_configuration.ApiBaseUrl);

            return httpClient;
        }
    }
}