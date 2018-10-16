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
                var startupTasks = container.GetAllInstances<IStartupTask>();
                
                await StartupTasks.StartAsync(startupTasks);
                
                var config = new JobHostConfiguration { JobActivator = new StructureMapJobActivator(container) };
                var isDevelopment = ConfigurationHelper.IsCurrentEnvironment(DasEnv.LOCAL);

                if (isDevelopment)
                {
                    config.UseDevelopmentSettings();
                }

                var host = new JobHost(config);
                
                host.Call(typeof(Program).GetMethod(nameof(RunAsync)));
                host.RunAndBlock();
                
                await StartupTasks.StopAsync();
            }
        }
        
        [NoAutomaticTrigger]
        public static async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(3000, cancellationToken);
            }
        }
    }
}