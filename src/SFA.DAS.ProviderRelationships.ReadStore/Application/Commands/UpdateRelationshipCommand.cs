using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    public class UpdateRelationshipCommand : IReadStoreRequest<Unit>
    {
        public long Ukprn { get; }
        public int AccountProviderId { get; }
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public HashSet<Operation> Operations { get; }
        public string MessageId { get; }
        public DateTime Updated { get; }

        public UpdateRelationshipCommand(long ukprn, int accountProviderId, long accountId,long accountLegalEntityId, 
            HashSet<Operation> grants, string messageId, DateTime updated)
        {
            Ukprn = ukprn;
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountProviderId = accountProviderId;
            Operations = grants;
            MessageId = messageId;
            Updated = updated;
        }
    }
}