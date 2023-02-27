using Microsoft.Azure.Documents;
using SFA.DAS.Http;
using SFA.DAS.PAS.Account.Api.ClientV2;
using SFA.DAS.PAS.Account.Api.ClientV2.Configuration;
using SFA.DAS.ProviderRelationships.Helpers;
using SFA.DAS.ProviderRelationships.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.ServiceRegistrations;

public static class ApplicationServiceRegistrations
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IProviderUrls, ProviderUrls>();
        services.AddTransient<IPasAccountApiClient>(CreateClient);
        services.AddTransient<IDocumentClientFactory, DocumentClientFactory>();
       
        services.AddSingleton<IDocumentClient>(provider =>
        {
            var factory = provider.GetService<IDocumentClientFactory>();
            return factory.CreateDocumentClient();
        });

        services.AddTransient<IAccountProviderLegalEntitiesRepository, AccountProviderLegalEntitiesRepository>();
        
        return services;
    }

    private static IPasAccountApiClient CreateClient(IServiceProvider ctx)
    {
        var config = ctx.GetService<PasAccountApiConfiguration>();
        var loggerFactory = ctx.GetService<ILoggerFactory>();

        var factory = new PasAccountApiClientFactory(config, loggerFactory);
        return factory.CreateClient();
    }
}

internal class PasAccountApiClientFactory
{
    private readonly PasAccountApiConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    public PasAccountApiClientFactory(PasAccountApiConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
    }

    public IPasAccountApiClient CreateClient()
    {
        IHttpClientFactory httpClientFactory;

        if (IsClientCredentialConfiguration(_configuration.ClientId, _configuration.ClientSecret, _configuration.Tenant))
        {
            httpClientFactory = new AzureActiveDirectoryHttpClientFactory(_configuration, _loggerFactory);
        }
        else
        {
            httpClientFactory = new ManagedIdentityHttpClientFactory(_configuration, _loggerFactory);
        }

        var httpClient = httpClientFactory.CreateHttpClient();

        var restHttpClient = new RestHttpClient(httpClient);
        var apiClient = new PasAccountApiClient(restHttpClient);

        return apiClient;
    }

    private static bool IsClientCredentialConfiguration(string clientId, string clientSecret, string tenant)
    {
        return !string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret) && !string.IsNullOrWhiteSpace(tenant);
    }
}