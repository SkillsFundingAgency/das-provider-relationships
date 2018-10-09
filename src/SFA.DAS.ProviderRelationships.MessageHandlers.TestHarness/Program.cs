using SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.DependencyResolution;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = IoC.Initialize())
            {
                var nServiceBusConfig = new NServiceBusConfig(container);
                try
                {
                    nServiceBusConfig.Start();
                    nServiceBusConfig.Endpoint.Publish(new CreatedAccountEvent {AccountId = 1, Name = "name"}).GetAwaiter().GetResult();
                }
                finally
                {
                    nServiceBusConfig.Stop();
                }
            }
        }
    }
}
