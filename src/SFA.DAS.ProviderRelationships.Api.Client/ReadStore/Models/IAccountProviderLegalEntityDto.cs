using System;
using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Models
{
    //todo: when this is implemented by both docs, what should it be called?
    public interface IAccountProviderLegalEntityDto
    {
        long AccountLegalEntityId { get; }

        long Ukprn { get; }

        IEnumerable<Operation> Operations { get; }

        DateTime? Deleted { get; }
    }
}