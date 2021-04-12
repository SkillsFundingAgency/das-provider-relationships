using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class ProviderRegistration
    {
        [JsonProperty("UKPRN")]
        public int Ukprn { get; set; }
        [JsonProperty]
        public string LegalName { get; set; }
    }
}