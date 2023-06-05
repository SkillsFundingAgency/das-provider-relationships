using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.UnitOfWork.NServiceBus.Configuration;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.Extensions;

public static class ServiceCollectionExtensions
{
    private const string EndpointName = "SFA.DAS.ProviderRelationships.MessageHandlers";

    public static IServiceCollection AddNServiceBus(this IServiceCollection services)
    {
        return services
            .AddSingleton(provider =>
            {
                var providerRelationshipsConfiguration = provider.GetService<ProviderRelationshipsConfiguration>();
                var configuration = provider.GetService<IConfiguration>();
                var isLocal = configuration["EnvironmentName"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase);

                var endpointConfiguration = new EndpointConfiguration(EndpointName)
                    .UseErrorQueue($"{EndpointName}-errors")
                    .UseInstallers()
                    .UseOutbox()
                    .UseMessageConventions()
                    .UseUnitOfWork()
                    .UseNewtonsoftJsonSerializer()
                    .UseSqlServerPersistence(() => DatabaseExtensions.GetSqlConnection(providerRelationshipsConfiguration.DatabaseConnectionString))
                    .UseAzureServiceBusTransport(() => providerRelationshipsConfiguration.ServiceBusConnectionString, isLocal)
                    .UseServicesBuilder(new UpdateableServiceProvider(services));

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