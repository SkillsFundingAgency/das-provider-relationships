using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.AutoConfiguration.DependencyResolution;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.Microsoft
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProviderRelationshipsApiClient(this IServiceCollection services)
        {
            return services.AddAutoConfiguration()
                .AddHttp();
        }
    }
}