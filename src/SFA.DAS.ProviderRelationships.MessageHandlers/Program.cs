using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
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
                var instrumentationKey = ConfigurationManager.AppSettings["InstrumentationKey"];

                var environment = container.GetInstance<IEnvironment>();
                if (environment.IsCurrent(DasEnv.LOCAL))
                {
                    config.UseDevelopmentSettings();
                }

                config.LoggerFactory = new LoggerFactory()
                    .AddApplicationInsights(instrumentationKey, null)
                    .AddNLog();

                var jobHost = new JobHost(config);
                
                await startup.StartAsync();
                await jobHost.CallAsync(typeof(Program).GetMethod(nameof(Block)));
                
                jobHost.RunAndBlock();
                
                await startup.StopAsync();
            }
        }
        
        [NoAutomaticTrigger]
        public static async Task Block(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(3000, cancellationToken);
            }
        }
    }
}