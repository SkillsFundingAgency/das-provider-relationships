using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    public class ProviderPermissions : Document
    {
        public ProviderPermissions() : base(1, "ProviderPermissions")
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

        [JsonProperty("grantPermissions")]
        public IEnumerable<GrantPermission> GrantPermissions { get; set; }
    }
}