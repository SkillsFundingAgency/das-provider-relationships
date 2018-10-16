using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.NLog;
using SFA.DAS.NServiceBus.StructureMap;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.DependencyResolution;
using SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Scenarios;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness
{
    public class Program
    {
        public static async Task Main()
        {
            using (var container = IoC.Initialize())
            {
                var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness")
                    .UseAzureServiceBusTransport(() => container.GetInstance<ProviderRelationshipsConfiguration>().ServiceBusConnectionString)
                    .UseErrorQueue()
                    .UseInstallers()
                    .UseLicense(container.GetInstance<ProviderRelationshipsConfiguration>().NServiceBusLicense)
                    .UseNewtonsoftJsonSerializer()
                    .UseNLogFactory()
                    .UseStructureMapBuilder(container);
                
                var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
                
                container.Configure(c => c.For<IMessageSession>().Use(endpoint));
                
                var publisher = container.GetInstance<PublishAllEvents>();
                
                try
                {
                    await publisher.Run();
                }
                finally
                {
                    await endpoint.Stop();
                }
            }
        }
    }
}
