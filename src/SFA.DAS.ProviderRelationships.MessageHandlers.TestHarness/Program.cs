using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.DependencyResolution;
using SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Scenarios;

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
                
                var publisher = container.GetInstance<PublishAllEvents>();
                
                try
                {
                    await publisher.Run();
                }
                finally
                {
                    await startup.StartAsync();
                }
            }
        }
    }
}
