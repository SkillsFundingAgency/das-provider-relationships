using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.DependencyResolution;
using SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Scenarios;
using SFA.DAS.ProviderRelationships.Startup;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness
{
    public static class Program
    {
        public static async Task Main()
        {
            using (var container = IoC.Initialize())
            {
                var startup = container.GetInstance<IStartup>();

                await startup.StartAsync();
                
                var publishEmployerAccountsEventsScenario = container.GetInstance<PublishEmployerAccountsEventsScenario>();
                var publishProviderRelationshipsEventsScenario = container.GetInstance<PublishProviderRelationshipsEventsScenario>();

                try
                {
                    await publishEmployerAccountsEventsScenario.Run();
                    await publishProviderRelationshipsEventsScenario.Run();
                }
                finally
                {
                    await startup.StopAsync();
                }
            }
        }
    }
}
