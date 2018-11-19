using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class AccountLegalEntity
    {
        [JsonProperty("accountLegalEntityId")]
        public long AccountLegalEntityId { get; protected set; }

        [JsonProperty("accountLegalEntityPublicHashedId")]
        public string AccountLegalEntityPublicHashedId { get; protected set; }

        [JsonProperty("accountLegalEntityName")]
        public string AccountLegalEntityName { get; protected set; }

        [JsonConstructor]
        protected AccountLegalEntity()
        {
        }

        public AccountLegalEntity(long accountLegalEntityId, string accountLegalEntityPublicHashedId, string accountLegalEntityName)
        {
            AccountLegalEntityId = accountLegalEntityId;
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            AccountLegalEntityName = accountLegalEntityName;
        }
    }
}