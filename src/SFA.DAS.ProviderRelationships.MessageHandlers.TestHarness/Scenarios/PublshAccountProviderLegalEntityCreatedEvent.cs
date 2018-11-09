using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;

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
            long ukprn = 1001277;
            long accountId = 123;


            var createEvent = new AccountProviderLegalEntityCreatedEvent(ukprn, accountId, 22222, "HASHED123", "AccountName",
                3333, "HASH333", "LEName", 1234, "ProvderName", DateTime.Now); 
            
            await _messageSession.Publish(createEvent);

            var update1Event = new AccountProviderLegalEntityUserUpdatedPermissionsEvent(ukprn, accountId, Guid.NewGuid(), new HashSet<Operation> {Operation.CreateCohort}, DateTime.Now);

            await _messageSession.Publish(update1Event);

            var update2Event = new AccountProviderLegalEntityUserUpdatedPermissionsEvent(ukprn, accountId, Guid.NewGuid(), new HashSet<Operation>(), DateTime.Now);

            await _messageSession.Publish(update2Event);

            var update3Event = new AccountProviderLegalEntityUserUpdatedPermissionsEvent(ukprn, accountId, Guid.NewGuid(), new HashSet<Operation> { Operation.CreateCohort }, DateTime.Now);

            await _messageSession.Publish(update3Event);

            var deletedEvent = new AccountProviderLegalEntityDeletedEvent(ukprn, accountId, DateTime.Now);

            await _messageSession.Publish(deletedEvent);

        }
    }
}