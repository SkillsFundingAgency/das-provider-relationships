using System;
using SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.DependencyResolution;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerCommitments.Messages.Events;
using SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Scenarios;

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

                    new PublishAllEvents().Run(nServiceBusConfig).GetAwaiter().GetResult();
                }
                finally
                {
                    nServiceBusConfig.Stop();
                }
            }
        }
    }
}
