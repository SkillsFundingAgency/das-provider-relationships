using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class AccountLegalEntity
    {
        [JsonProperty("id")]
        public long Id { get; protected set; }

        [JsonProperty("publicHashedId")]
        public string PublicHashedId { get; protected set; }

        [JsonProperty("name")]
        public string Name { get; protected set; }

        [JsonConstructor]
        protected AccountLegalEntity()
        {
        }

        public AccountLegalEntity(long id, string publicHashedId, string name)
        {
            Id = id;
            PublicHashedId = publicHashedId;
            Name = name;
        }
    }
}