using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderUkprnsByAccountIdQueryResult
    {
        public IEnumerable<long> Ukprns { get; }

        public GetAccountProviderUkprnsByAccountIdQueryResult(IEnumerable<long> ukprns)
        {
            Ukprns = ukprns;
        }
    }
}