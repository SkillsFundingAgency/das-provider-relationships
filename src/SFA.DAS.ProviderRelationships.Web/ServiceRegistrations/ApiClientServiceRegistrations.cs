using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Api.Client.Http;
using SFA.DAS.ProviderRelationships.Api.Client;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services.OuterApi;

namespace SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;

public static class ApiClientServiceRegistrations
{
    public static IServiceCollection AddApiClients(this IServiceCollection services)
    {
        services.AddTransient<IRoatpApiHttpClientFactory, RoatpApiHttpClientFactory>();
        services.AddTransient<IRecruitApiHttpClientFactory, RecruitApiHttpClientFactory>();
        
        services.AddTransient<IProviderRelationshipsApiClientFactory, ProviderRelationshipsApiClientFactory>();

        services.AddTransient<IProviderRelationshipsApiClient>(provider =>
        {
            var factory = provider.GetService<IProviderRelationshipsApiClientFactory>();
            return factory.CreateApiClient();
        });

        services.AddHttpClient<IRegistrationApiClient, RegistrationApiClient>();
        services.AddTransient<IRegistrationApiClient, RegistrationApiClient>();

        services.AddTransient<IRestHttpClient, RestHttpClient>();

        services.AddHttpClient<IOuterApiClient, OuterApiClient>((p,x) =>
        {
            var outerApiConfiguration = p.GetService<OuterApiConfiguration>();
            x.BaseAddress = new Uri(outerApiConfiguration.BaseUrl);
        });

        return services;
    }
}