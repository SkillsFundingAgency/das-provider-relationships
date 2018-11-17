using System;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class AccountProviderLegalEntity
    {
        [JsonProperty("accountProviderLegalEntityId")]
        public long AccountProviderLegalEntityId { get; protected set; }

        [JsonProperty("accountLegalEntityId")]
        public long AccountLegalEntityId { get; protected set; }

        [JsonProperty("accountLegalEntityPublicHashedId")]
        public string AccountLegalEntityPublicHashedId { get; protected set; }

        [JsonProperty("accountLegalEntityName")]
        public string AccountLegalEntityName { get; protected set; }

        [JsonProperty("created")]
        public DateTime Created { get; protected set; }

        [JsonConstructor]
        protected AccountProviderLegalEntity()
        {
        }

        public AccountProviderLegalEntity(long accountProviderLegalEntityId, long accountLegalEntityId, string accountLegalEntityPublicHashedId, string accountLegalEntityName, DateTime created)
        {
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            AccountLegalEntityName = accountLegalEntityName;
            Created = created;
        }
    }
}