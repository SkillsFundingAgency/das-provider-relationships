using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Types.ReadStore.Models
{
    public interface IAccountProviderLegalEntityDto
    {
        long AccountLegalEntityId { get; }
        long Ukprn { get; }
        IEnumerable<Operation> Operations { get; }
        DateTime? Deleted { get; }
    }
}