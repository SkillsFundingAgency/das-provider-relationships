using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.Client.Http;

public class ProviderRelationshipsApiClientFactory : IProviderRelationshipsApiClientFactory
{
    private readonly ProviderRelationshipsApiConfiguration _configuration;

    public ProviderRelationshipsApiClientFactory(ProviderRelationshipsApiConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IProviderRelationshipsApiClient CreateApiClient()
    {
        var httpClientFactory = GetHttpClientFactory();
        var httpClient = httpClientFactory.CreateHttpClient();
        var restHttpClient = new RestHttpClient(httpClient);
        var apiClient = new ProviderRelationshipsApiClient(restHttpClient);

        return apiClient;
    }

    private IHttpClientFactory GetHttpClientFactory()
    {
        // cannot use a ternary conditional operator here as it causes a build failure in the azure pipeline
        if (IsClientCredentialConfiguration(_configuration.ClientId, _configuration.ClientSecret, _configuration.Tenant))
        {
            return new AzureActiveDirectoryHttpClientFactory(_configuration);
        }

        return new ManagedIdentityHttpClientFactory(_configuration);
    }

    private static bool IsClientCredentialConfiguration(string clientId, string clientSecret, string tenant)
    {
        return !string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret) && !string.IsNullOrEmpty(tenant);
    }
}