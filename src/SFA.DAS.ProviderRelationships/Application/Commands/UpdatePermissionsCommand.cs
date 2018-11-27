using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class UpdatePermissionsCommand : IRequest
    {
        public long AccountId { get; }
        public long AccountProviderId { get; }
        public long AccountLegalEntityId { get; }
        public Guid UserRef { get; }
        public HashSet<Operation> GrantedOperations { get; }

        public UpdatePermissionsCommand(long accountId, long accountProviderId, long accountLegalEntityId, Guid userRef, HashSet<Operation> grantedOperations)
        {
            AccountId = accountId;
            AccountProviderId = accountProviderId;
            AccountLegalEntityId = accountLegalEntityId;
            UserRef = userRef;
            GrantedOperations = grantedOperations;
        }
    }
}