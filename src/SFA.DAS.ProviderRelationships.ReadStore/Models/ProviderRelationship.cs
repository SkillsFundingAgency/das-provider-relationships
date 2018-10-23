using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    public class Permisson
    {
        [JsonProperty("ukprn")]
        public long Ukprn { get; set; }

        [JsonProperty("employerAccountId")]
        public long EmployerAccountId { get; set; }

        [JsonProperty("legalEntityHashedId")]
        public string LegalEntityHashedId { get; set; }

        [JsonProperty("legalEntityName")]
        public string LegalEntityName { get; set; }

        [JsonProperty("permissions")]
        public IEnumerable<Permission> Permissions { get; set; }
    }
}


