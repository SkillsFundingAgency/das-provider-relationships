using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Jobs.DependencyResolution;
using SFA.DAS.ProviderRelationships.Jobs.StartupJobs;
using SFA.DAS.ProviderRelationships.Startup;

namespace SFA.DAS.ProviderRelationships.Jobs
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
                    .AddNLog();

                config.UseTimers();

                var jobHost = new JobHost(config);
                
                await startup.StartAsync();
                await jobHost.CallAsync(typeof(CreateReadStoreDatabaseJob).GetMethod(nameof(CreateReadStoreDatabaseJob.Run)));
                
                jobHost.RunAndBlock();
                
                await startup.StopAsync();
            }
        }
    }
}