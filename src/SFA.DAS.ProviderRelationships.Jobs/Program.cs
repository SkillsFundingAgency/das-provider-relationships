using SFA.DAS.ProviderRelationships.Jobs.Extensions;

namespace SFA.DAS.ProviderRelationships.Jobs;

public static class Program
{
    public static async Task Main()
    {
        using (var host = CreateHost())
        {
            await host.RunAsync();
        }
    }


    private static IHost CreateHost()
    {
        return new HostBuilder()
            .ConfigureDasAppConfiguration()
            .ConfigureDasWebJobs()
            .ConfigureDasLogging()
            .ConfigureDasServices()
            .Build();
    }
}