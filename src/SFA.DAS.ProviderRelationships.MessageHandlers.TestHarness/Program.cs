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
                
                var scenario = container.GetInstance<PublishEmployerAccountsEvents>();
                //var scenario = container.GetInstance<PublishProviderRelationshipsEvents>();

                try
                {
                    await scenario.Run();
                }
                finally
                {
                    await startup.StopAsync();
                }
            }
        }
    }
}
