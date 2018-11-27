using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    public class UpdatePermissionsCommand : IReadStoreRequest<Unit>
    {
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public long AccountProviderId { get; }
        public long AccountProviderLegalEntityId { get; }
        public long Ukprn { get; }
        public HashSet<Operation> GrantedOperations { get; }
        public DateTime Updated { get; }
        public string MessageId { get; }

        public UpdatePermissionsCommand(long accountId, long accountLegalEntityId, long accountProviderId, long accountProviderLegalEntityId, long ukprn, HashSet<Operation> grantedOperations, DateTime updated, string messageId)
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountProviderId = accountProviderId;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            GrantedOperations = grantedOperations;
            Updated = updated;
            MessageId = messageId;
        }
    }
}