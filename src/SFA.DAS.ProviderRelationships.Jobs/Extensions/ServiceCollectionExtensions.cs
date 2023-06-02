using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;

namespace SFA.DAS.ProviderRelationships.Jobs.Extensions;

public static class ServiceCollectionExtensions
{
    private const string EndpointName = "SFA.DAS.ProviderRelationships.Jobs";

    public static IServiceCollection AddNServiceBus(this IServiceCollection services)
    {
        return services
            .AddSingleton(p =>
            {
                var providerRelationshipsConfiguration = p.GetService<ProviderRelationshipsConfiguration>();
                var configuration = p.GetService<IConfiguration>();
                var isLocal = configuration["EnvironmentName"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase);

                var endpointConfiguration = new EndpointConfiguration(EndpointName)
                    .UseErrorQueue($"{EndpointName}-errors")
                    .UseInstallers()
                    .UseSendOnly()
                    .UseMessageConventions()
                    .UseNewtonsoftJsonSerializer()
                    .UseSqlServerPersistence(() => DatabaseExtensions.GetSqlConnection(providerRelationshipsConfiguration.DatabaseConnectionString))
                    .UseAzureServiceBusTransport(() => providerRelationshipsConfiguration.ServiceBusConnectionString, isLocal);

                if (!string.IsNullOrEmpty(providerRelationshipsConfiguration.NServiceBusLicense))
                {
                    endpointConfiguration.UseLicense(providerRelationshipsConfiguration.NServiceBusLicense);
                }
                
                var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

                return endpoint;
            })
            .AddSingleton<IMessageSession>(s => s.GetService<IEndpointInstance>())
            .AddHostedService<NServiceBusHostedService>();
    }
}