using System;
using System.Net.Http;
using SFA.DAS.Http;
using SFA.DAS.Http.TokenGenerators;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.Client.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly ProviderRelationshipsApiClientConfiguration _configuration;

        public HttpClientFactory(ProviderRelationshipsApiClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        public HttpClient CreateHttpClient()
        {
            var httpClientBuilder = new HttpClientBuilder()
                .WithDefaultHeaders();

            // don't want to add dependency on ProviderRelationships for IEnvironment
//            var environment = container.GetInstance<IEnvironmentService>();
//            if (environment.IsCurrent(DasEnv.LOCAL))

//todo: replace this dangerous #if with environmentservice's environment.IsCurrent(DasEnv.LOCAL) when the package is available
#if !DEBUG
            httpClientBuilder.WithBearerAuthorisationHeader(new AzureADBearerTokenGenerator(_configuration));
#endif

            var httpClient = httpClientBuilder.Build();
            
            httpClient.BaseAddress = new Uri(_configuration.ApiBaseUrl);

            return httpClient;
        }
    }
}