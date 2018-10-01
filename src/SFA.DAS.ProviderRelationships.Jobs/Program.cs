using Microsoft.Azure.WebJobs;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Jobs.DependencyResolution;

namespace SFA.DAS.ProviderRelationships.Jobs
{
    public static class Program
    {
        public static void Main()
        {
            using (var container = IoC.Initialize())
            {
                var config = new JobHostConfiguration
                {
                    JobActivator = new StructureMapJobActivator(container)
                };

                var isDevelopment = ConfigurationHelper.IsCurrentEnvironment(DasEnv.LOCAL);

                if (isDevelopment)
                {
                    config.UseDevelopmentSettings();
                }

                config.UseTimers();

                var host = new JobHost(config);

                host.Call(typeof(EndpointJob).GetMethod(nameof(EndpointJob.RunAsync)));
                host.RunAndBlock();
            }
        }
    }
}