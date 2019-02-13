using System.Collections.Generic;
using MediatR;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class UpdatedPermissionsEventNotifyCommand : IRequest
    {
        public long AccountProviderId { get; set; }
        public long Ukprn { get; }
        public HashSet<Operation> GrantedOperations { get; }

        public UpdatedPermissionsEventNotifyCommand
        (
            long accountProviderId,
            long ukprn,
            HashSet<Operation> grantedOperations
        )
        {
            AccountProviderId = accountProviderId;
            Ukprn = ukprn;
            GrantedOperations = grantedOperations;
        }
    }
}
