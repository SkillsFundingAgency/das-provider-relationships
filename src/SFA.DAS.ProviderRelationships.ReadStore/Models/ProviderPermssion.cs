using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    public class ProviderPermission : Document
    {
        public ProviderPermission() : base(1, "ProviderPermission")
        {   
        }

        [JsonProperty("ukprn")]
        public long Ukprn { get; set; }

        [JsonProperty("employerAccountId")]
        public long EmployerAccountId { get; set; }

        [JsonProperty("employerAccountLegalEntityPublicHashedId")]
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }

        [JsonProperty("employerAccountLegalEntityName")]
        public string EmployerAccountLegalEntityName { get; set; }

        [JsonProperty("operations")]
        public IEnumerable<Operation> Operations { get; set; }
    }
}