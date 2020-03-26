using System.Collections.Generic;
using MediatR;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.SendUpdatedPermissionsNotification
{
    public class SendUpdatedPermissionsNotificationCommand : IRequest
    {
        public long Ukprn { get; set; }
        public long AccountLegalEntityId { get; set; }
        public HashSet<Operation> PreviousOperations { get; set; }
        public HashSet<Operation> GrantedOperations { get; set; }

        public SendUpdatedPermissionsNotificationCommand
        (
            long ukprn,
            long accountLegalEntityId
,
            HashSet<Operation> previousOperations,
            HashSet<Operation> grantedOperations)
        {
            Ukprn = ukprn;
            AccountLegalEntityId = accountLegalEntityId;
            PreviousOperations = previousOperations;
            GrantedOperations = grantedOperations;
        }
    }
}
