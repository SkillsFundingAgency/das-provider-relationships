using System.Collections.Generic;
using MediatR;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.RevokePermissions
{
    public class RevokePermissionsCommand : IRequest
    {
        public long Ukprn { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
        public IEnumerable<Operation> OperationsToRemove { get; set; }

        public RevokePermissionsCommand(
            long ukprn,
            string accountLegalEntityPublicHashedId,
            IEnumerable<Operation> operationsToRemove)
        {
            Ukprn = ukprn;
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            OperationsToRemove = operationsToRemove;
        }
    }
}
