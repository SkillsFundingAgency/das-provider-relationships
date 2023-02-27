using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.ProviderRelationships.MessageHandlers.Extensions;

namespace SFA.DAS.ProviderRelationships.MessageHandlers;

public static class Program
{
    public static async Task Main(string[] args)
    {
        using (var host = CreateHost(args))
        {
            await host.RunAsync();
        }
    }

    private static IHost CreateHost(string[] args)
    {
        var builder = new HostBuilder()
            .ConfigureDasAppConfiguration()
            .UseDasEnvironment()
            .UseConsoleLifetime()
            .ConfigureDasLogging()
            .ConfigureDasServices()
            .UseNServiceBusContainer();

        return builder.Build();
    }
}