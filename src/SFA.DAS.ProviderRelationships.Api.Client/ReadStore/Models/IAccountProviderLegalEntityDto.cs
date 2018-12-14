using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models
{
    public interface IAccountProviderLegalEntityDto
    {
        long AccountLegalEntityId { get; }

        long Ukprn { get; }

        IEnumerable<Operation> Operations { get; }

        DateTime? Deleted { get; }
    }
}