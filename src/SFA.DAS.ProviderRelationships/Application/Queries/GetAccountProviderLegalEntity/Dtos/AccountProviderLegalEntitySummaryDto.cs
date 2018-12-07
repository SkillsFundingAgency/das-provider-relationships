using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity.Dtos
{
    public class AccountProviderLegalEntitySummaryDto
    {
        public long Id { get; set; }
        public long AccountLegalEntityId { get; set; }
        public List<Operation> Operations { get; set; }
    }
}