using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Messages.Events;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Scenarios
{
    public class PublshAccountProviderLegalEntityCreatedEvent
    {
        private readonly IMessageSession _messageSession;

        public PublshAccountProviderLegalEntityCreatedEvent(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public async Task Run()
        {
            var @event = new AccountProviderLegalEntityCreatedEvent(1001222, 123, 22222, "HASHED123", "AccountName",
                3333, "HASH333", "LEName", 1234, "ProvderName", DateTime.Now); 
            
            await _messageSession.Publish(@event);

        }
    }
}