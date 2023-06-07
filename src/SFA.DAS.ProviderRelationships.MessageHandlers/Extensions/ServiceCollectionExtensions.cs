using System.Net;
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
                var configuration = provider.GetService<ProviderRelationshipsConfiguration>();
                var hostingEnvironment = provider.GetService<IHostEnvironment>();
                var isDevelopment = hostingEnvironment.IsDevelopment();

                var endpointConfiguration = new EndpointConfiguration(EndpointName)
                    .UseAzureServiceBusTransport(() => configuration.ServiceBusConnectionString, isDevelopment)
                    .UseErrorQueue($"{EndpointName}-errors")
                    .UseInstallers()
                    .UseSqlServerPersistence(() => DatabaseExtensions.GetSqlConnection(configuration.DatabaseConnectionString))
                    .UseNewtonsoftJsonSerializer()
                    .UseOutbox()
                    .UseUnitOfWork()
                    .UseServicesBuilder(new UpdateableServiceProvider(services));

                if (!string.IsNullOrEmpty(configuration.NServiceBusLicense))
                {
                    var decodedLicence = WebUtility.HtmlDecode(configuration.NServiceBusLicense);
                    endpointConfiguration.UseLicense(decodedLicence);
                }

                var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

                return endpoint;
            })
            .AddSingleton<IMessageSession>(s => s.GetService<IEndpointInstance>())
            .AddHostedService<NServiceBusHostedService>();
    }
}