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
            long ukprn = 2001677;
            int accountProviderId = 2;
            long accountId = 2134;
            long accountLegalEntityId = 455;

            var createEvent = new AccountProviderLegalEntityUpdatedPermissionsEvent(ukprn, 
                accountProviderId, "AccountProviderName",  accountId, "HASHAC", "AcountNmae", 
                accountLegalEntityId, "HASHED123", "AccountLEName",
                new HashSet<Operation>(), 
                DateTime.Now); 
            
            await _messageSession.Publish(createEvent);

            var updateEvent = new AccountProviderLegalEntityUpdatedPermissionsEvent(ukprn,
                accountProviderId, "AccountProviderName", accountId, "HASHAC", "AcountNmae",
                accountLegalEntityId, "HASHED123", "AccountLEName",
                new HashSet<Operation>{Operation.CreateCohort},
                DateTime.Now);

            await _messageSession.Publish(updateEvent);
        }
    }
}