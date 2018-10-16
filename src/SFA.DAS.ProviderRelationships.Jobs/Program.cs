using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Jobs.DependencyResolution;

namespace SFA.DAS.ProviderRelationships.Jobs
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

                config.UseTimers();

                var host = new JobHost(config);
                
                host.RunAndBlock();

                await StartupTasks.StopAsync();
            }
        }
    }
}