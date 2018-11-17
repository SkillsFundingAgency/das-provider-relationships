using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAccountProviderUkprnsQueryResult
    {
        public IEnumerable<long> Ukprns { get; }

        public GetAccountProviderUkprnsQueryResult(IEnumerable<long> ukprns)
        {
            Ukprns = ukprns;
        }
    }
}