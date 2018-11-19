using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class Provider
    {
        [JsonProperty("ukprn")]
        public long Ukprn { get; protected set; }

        [JsonConstructor]
        protected Provider()
        {
        }

        public Provider(long ukprn)
        {
            Ukprn = ukprn;
        }
    }
}