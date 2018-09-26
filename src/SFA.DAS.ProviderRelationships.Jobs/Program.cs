using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.NLog;
using SFA.DAS.NServiceBus.SqlServer;
using SFA.DAS.NServiceBus.StructureMap;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Jobs.DependencyResolution;

namespace SFA.DAS.ProviderRelationships.Jobs
{
    public class Program
    {
        public static void Main()
        {
            var isDevelopment = ConfigurationHelper.IsCurrentEnvironment(DasEnv.LOCAL);
            var config = new JobHostConfiguration();

            if (isDevelopment)
            {
                config.UseDevelopmentSettings();
            }
            
            config.UseTimers();

            var host = new JobHost(config);

            host.Call(typeof(Program).GetMethod(nameof(AsyncMain)));
            host.RunAndBlock();
        }

        [NoAutomaticTrigger]
        public static async Task AsyncMain(CancellationToken cancellationToken)
        {
            var container = IoC.Initialize();

            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ProviderRelationships.Jobs")
                .UseAzureServiceBusTransport(() => container.GetInstance<ProviderRelationshipsConfiguration>().ServiceBusConnectionString)
                .UseLicense(container.GetInstance<ProviderRelationshipsConfiguration>().NServiceBusLicense.HtmlDecode())
                .UseSqlServerPersistence(() => container.GetInstance<DbConnection>())
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                .UseSendOnly()
                .UseStructureMapBuilder(container);

            var endpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            container.Configure(c =>
            {
                c.For<IMessageSession>().Use(endpoint);
            });

            ServiceLocator.Initialize(container);
        }
    }
}