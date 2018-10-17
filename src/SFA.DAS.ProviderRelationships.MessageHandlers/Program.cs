using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.MessageHandlers.DependencyResolution;

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

                if (isDevelopment)
                {
                    config.UseDevelopmentSettings();
                }

                var host = new JobHost(config);
                
                await startup.StartAsync();
                await host.CallAsync(typeof(Program).GetMethod(nameof(BlockAsync)));
                await host.StopAsync();
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