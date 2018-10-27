using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.MessageHandlers.DependencyResolution;
using SFA.DAS.ProviderRelationships.Startup;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public static class Program
    {
        public static async Task Main()
        {
            using (var container = IoC.Initialize())
            {
                var startup = container.GetInstance<IStartup>();
                var config = new JobHostConfiguration { JobActivator = new StructureMapJobActivator(container) };
                var isDevelopment = ConfigurationHelper.IsCurrentEnvironment(DasEnv.LOCAL);
                var instrumentationKey = ConfigurationManager.AppSettings["InstrumentationKey"];

                if (isDevelopment)
                {
                    config.UseDevelopmentSettings();
                }

                config.LoggerFactory = new LoggerFactory()
                    .AddApplicationInsights(instrumentationKey, null)
                    .AddConsole();

                var host = new JobHost(config);
                
                await startup.StartAsync();
                host.Call(typeof(Program).GetMethod(nameof(BlockAsync)));
                host.RunAndBlock();
                await startup.StopAsync();
            }
        }
        
        [NoAutomaticTrigger]
        public static async Task BlockAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(3000, cancellationToken);
            }
        }
    }
}