using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Audit.Commands
{
    public class UpdatedPermissionsEventAuditCommand : IRequest
    {
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public long AccountProviderId { get; }
        public long AccountProviderLegalEntityId { get; }
        public long Ukprn { get; }
        public Guid UserRef { get; }
        public HashSet<Operation> GrantedOperations { get; }
        public DateTime Updated { get; }

        public UpdatedPermissionsEventAuditCommand
        (
            long accountId,
            long accountLegalEntityId,
            long accountProviderId,
            long accountProviderLegalEntityId,
            long ukprn,
            Guid userRef,
            HashSet<Operation> grantedOperations,
            DateTime updated
        )
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountProviderId = accountProviderId;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            UserRef = userRef;
            GrantedOperations = grantedOperations;
            Updated = updated;
        }
    }
}