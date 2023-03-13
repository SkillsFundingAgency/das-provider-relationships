using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Configuration.NLog;
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
            .AddSingleton(p =>
            {
                var providerRelationshipsConfiguration = p.GetService<ProviderRelationshipsConfiguration>();
                var configuration = p.GetService<IConfiguration>();
                var isLocal = configuration["EnvironmentName"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase);

                var endpointConfiguration = new EndpointConfiguration(EndpointName)
                    .UseAzureServiceBusTransport(() => providerRelationshipsConfiguration.ServiceBusConnectionString, isLocal)
                    .UseErrorQueue($"{EndpointName}-errors")
                    .UseInstallers()
                    .UseMessageConventions()
                    .UseSqlServerPersistence(() => DatabaseExtensions.GetSqlConnection(providerRelationshipsConfiguration.DatabaseConnectionString))
                    .UseNewtonsoftJsonSerializer()
                    .UseNLogFactory()
                    .UseOutbox()
                    .UseServicesBuilder(new UpdateableServiceProvider(services))
                    .UseUnitOfWork();

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