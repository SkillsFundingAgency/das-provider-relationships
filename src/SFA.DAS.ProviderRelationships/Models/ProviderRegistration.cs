using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class ProviderRegistration
    {
        [JsonProperty("UKPRN")]
        public int Ukprn { get; set; }
        [JsonProperty("legalName")]
        public string ProviderName { get; set; }
    }
}