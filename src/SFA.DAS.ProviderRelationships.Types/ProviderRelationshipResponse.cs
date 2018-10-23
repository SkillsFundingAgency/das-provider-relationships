using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Types
{
    public class ProviderRelationshipResponse
    {
        public IEnumerable<ProviderRelationship> ProviderRelationships { get; set; }

        public class ProviderRelationship
        {
            public long Ukprn { get; set; }
            public long EmployerAccountId { get; set; }
            public string EmployerName { get; set; }
            public string EmployerAccountLegalEntityPublicHashedId { get; set; }
            public string EmployerAccountLegalEntityName { get; set; }
        }

    }

}