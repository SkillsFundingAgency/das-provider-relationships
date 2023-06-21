using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Extensions;

public static class ServiceCollectionExtensions
{
    private const string EndpointName = "SFA.DAS.ProviderRelationships.MessageHandlers";

    public static IServiceCollection AddNServiceBus(this IServiceCollection services)
    {
        return services
            .AddSingleton(p =>
            {
                var relationshipsConfiguration = p.GetService<ProviderRelationshipsConfiguration>();
                var configuration = p.GetService<IConfiguration>();
                var isLocal = configuration["EnvironmentName"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase);

                var endpointConfiguration = new EndpointConfiguration(EndpointName)
                        .ConfigureServiceBusTransport(() => relationshipsConfiguration.ServiceBusConnectionString, isLocal)
                    .UseErrorQueue($"{EndpointName}-errors")
                    .UseInstallers()
                    .UseMessageConventions()
                    .UseNewtonsoftJsonSerializer()
                    .UseSendOnly();

                var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

                return endpoint;
            })
            .AddSingleton<IMessageSession>(s => s.GetService<IEndpointInstance>())
            .AddHostedService<NServiceBusHostedService>();
    }
}