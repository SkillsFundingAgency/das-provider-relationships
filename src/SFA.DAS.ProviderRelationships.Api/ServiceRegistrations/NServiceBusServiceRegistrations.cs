using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.NServiceBus.SqlServer.Configuration;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.UnitOfWork.NServiceBus.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.ServiceRegistrations;

public static class NServiceBusServiceRegistrations
{
    private const string EndPointName = "SFA.DAS.ProviderRelationships.Api";

    public static void StartNServiceBus(this UpdateableServiceProvider services, IConfiguration configuration, bool isDevOrLocal)
    {
        var providerRelationshipsConfiguration = configuration.Get<ProviderRelationshipsConfiguration>();

        var databaseConnectionString = providerRelationshipsConfiguration.DatabaseConnectionString;

        if (string.IsNullOrWhiteSpace(databaseConnectionString))
        {
            throw new InvalidOperationException("DatabaseConnectionString configuration value is empty.");
        }

        var endpointConfiguration = new EndpointConfiguration(EndPointName)
            .UseErrorQueue($"{EndPointName}-errors")
            .UseInstallers()
            .UseLicense(providerRelationshipsConfiguration.NServiceBusLicense)
            .UseMessageConventions()
            .UseSqlServerPersistence(() => DatabaseExtensions.GetSqlConnection(providerRelationshipsConfiguration.DatabaseConnectionString))
            .UseNewtonsoftJsonSerializer()
            .UseOutbox()
            .UseUnitOfWork();

        if (isDevOrLocal)
        {
            endpointConfiguration.UseLearningTransport();
        }
        else
        {
            endpointConfiguration.UseAzureServiceBusTransport(providerRelationshipsConfiguration.ServiceBusConnectionString, r => { });
        }

        if (!string.IsNullOrEmpty(providerRelationshipsConfiguration.NServiceBusLicense))
        {
            endpointConfiguration.License(providerRelationshipsConfiguration.NServiceBusLicense);
        }

        var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        services.AddSingleton(p => endpoint)
            .AddSingleton<IMessageSession>(p => p.GetService<IEndpointInstance>())
            .AddHostedService<NServiceBusHostedService>();
    }
}