using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Scenarios
{
    public class PublishProviderRelationshipsEventsScenario
    {
        private readonly IMessageSession _messageSession;

        public PublishProviderRelationshipsEventsScenario(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public async Task Run()
        {
            var userRef = Guid.NewGuid();
            var correlationId = Guid.NewGuid(); 
            var userEmail = "a@a.com";
            var userFirstName = "a";
            var userLastName = "a";
            const long accountId = 1;
            const long accountLegalEntityId = 2;
            const long accountProviderId = 4;
            const long accountProviderLegalEntityId = 34;
            const long ukprn = 12345678;

            await _messageSession.Publish(new UpdatedPermissionsEvent(
                accountId,
                accountLegalEntityId,
                accountProviderId,
                accountProviderLegalEntityId,
                ukprn,
                userRef,
                userEmail,
                userFirstName,
                userLastName,
                new HashSet<Operation>(),
                DateTime.UtcNow));

            await _messageSession.Publish(new DeletedPermissionsEventV2(
                accountProviderLegalEntityId,
                ukprn,
                accountLegalEntityId,
                DateTime.UtcNow));

            await _messageSession.Publish(new AddedAccountProviderEvent(
                accountProviderId,
                accountId,
                ukprn,
                userRef,
                DateTime.UtcNow,
                correlationId));
        }
    }
}