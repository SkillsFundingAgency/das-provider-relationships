using SFA.DAS.ProviderRelationships.Jobs.Extensions;

namespace SFA.DAS.ProviderRelationships.Jobs;

public static class Program
{

    private static IHost CreateHost()
    {
        return new HostBuilder()
            .ConfigureDasAppConfiguration()
            .ConfigureDasWebJobs()
            .ConfigureDasLogging()
            .ConfigureDasServices()
            .Build();
    }

    //public static async Task Main()
    //{
    //    using (var container = IoC.Initialize())
    //    {
    //        ServicePointManager.DefaultConnectionLimit = 50;

    //        var config = new JobHostConfiguration { JobActivator = new StructureMapJobActivator(container) };
    //        var environmentService = container.GetInstance<IEnvironmentService>();
    //        var loggerFactory = container.GetInstance<ILoggerFactory>();
    //        var startup = container.GetInstance<IStartup>();

    //        if (environmentService.IsCurrent(DasEnv.LOCAL))
    //        {
    //            config.UseDevelopmentSettings();
    //        }

    //        config.LoggerFactory = loggerFactory;

    //        config.UseTimers();

    //        var jobHost = new JobHost(config);

    //        await startup.StartAsync();
    //        await jobHost.CallAsync(typeof(CreateReadStoreDatabaseJob).GetMethod(nameof(CreateReadStoreDatabaseJob.Run)));

    //        jobHost.RunAndBlock();

    //        await startup.StopAsync();
    //    }
    //}
}