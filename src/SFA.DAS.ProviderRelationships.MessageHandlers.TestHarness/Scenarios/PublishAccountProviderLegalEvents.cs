using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Scenarios
{
    public class PublishAccountProviderLegalEvents
    {
        private readonly IMessageSession _messageSession;

        public PublishAccountProviderLegalEvents(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public async Task Run()
        {
            long ukprn = 5055678;
            int accountProviderId = 1;
            long accountLegalEntityId = 2134;

            var createEvent = new AccountProviderLegalEntityCreatedEvent(ukprn, 2, "HASHED123", "AccountName",
                accountLegalEntityId, "HASH333", "LEName", accountProviderId, "ProvderName", DateTime.Now); 
            
            await _messageSession.Publish(createEvent);

            var update1Event = new AccountProviderLegalEntityUserUpdatedPermissionsEvent(ukprn, accountProviderId, accountLegalEntityId, Guid.NewGuid(), new HashSet<Operation> {Operation.CreateCohort}, DateTime.Now);

            await _messageSession.Publish(update1Event);

            var update2Event = new AccountProviderLegalEntityUserUpdatedPermissionsEvent(ukprn, accountProviderId, accountLegalEntityId, Guid.NewGuid(), new HashSet<Operation>(), DateTime.Now);

            await _messageSession.Publish(update2Event);

            var update3Event = new AccountProviderLegalEntityUserUpdatedPermissionsEvent(ukprn, accountProviderId, accountLegalEntityId, Guid.NewGuid(), new HashSet<Operation> { Operation.CreateCohort }, DateTime.Now);

            await _messageSession.Publish(update3Event);

            var deletedEvent = new AccountProviderLegalEntityDeletedEvent(ukprn, accountProviderId, accountLegalEntityId, DateTime.Now);

            await _messageSession.Publish(deletedEvent);
        }
    }
}