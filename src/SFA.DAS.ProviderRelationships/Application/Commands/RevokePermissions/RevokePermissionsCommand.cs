﻿using System.Collections.Generic;
using System.Linq;
using MediatR;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.RevokePermissions
{
    public class RevokePermissionsCommand : IRequest
    {
        public long Ukprn { get; }
        public string AccountLegalEntityPublicHashedId { get; }
        public List<Operation> OperationsToRevoke { get; }

        public RevokePermissionsCommand(
            long ukprn,
            string accountLegalEntityPublicHashedId,
            IEnumerable<Operation> operationsToRevoke)
        {
            Ukprn = ukprn;
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            OperationsToRevoke = operationsToRevoke.ToList();
        }
    }
}
