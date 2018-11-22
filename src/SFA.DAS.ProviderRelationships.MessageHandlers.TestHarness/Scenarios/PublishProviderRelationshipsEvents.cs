using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Scenarios
{
    public class PublishProviderRelationshipsEvents
    {
        private readonly IMessageSession _messageSession;

        public PublishProviderRelationshipsEvents(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public async Task Run()
        {
            var userRef = Guid.NewGuid();
            const long accountId = 2134;
            const long accountLegalEntityId = 455;
            const long accountProviderId = 2;
            const long ukprn = 2001677;

            await _messageSession.Publish(new UpdatedPermissionsEvent(
                accountId,
                accountLegalEntityId,
                accountProviderId,
                ukprn,
                userRef,
                new HashSet<Operation>(),
                DateTime.UtcNow));

            await _messageSession.Publish(new UpdatedPermissionsEvent(
                accountId,
                accountLegalEntityId,
                accountProviderId,
                ukprn,
                userRef,
                new HashSet<Operation> { Operation.CreateCohort },
                DateTime.UtcNow));
        }
    }
}