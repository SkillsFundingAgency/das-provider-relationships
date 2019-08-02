using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRelationships.Api.Client.Http;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.Microsoft
{
    internal static class HttpServiceCollectionExtensions
    {
        public static IServiceCollection AddHttp(this IServiceCollection services)
        {
            return services.AddSingleton(p => p.GetRequiredService<IProviderRelationshipsApiClientFactory>().CreateApiClient())
                .AddTransient<IProviderRelationshipsApiClientFactory, ProviderRelationshipsApiClientFactory>();
        }
    }
}