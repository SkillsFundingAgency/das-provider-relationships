using System;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class AccountProvider
    {
        [JsonProperty("ukprn")]
        public long Ukprn { get; protected set; }

        [JsonProperty("accountProviderId")]
        public long AccountProviderId { get; protected set; }

        [JsonProperty("accountId")]
        public long AccountId { get; protected set; }

        [JsonProperty("accountPublicHashedId")]
        public string AccountPublicHashedId { get; protected set; }

        [JsonProperty("accountName")]
        public string AccountName { get; protected set; }

        [JsonProperty("created")]
        public DateTime Created { get; protected set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; protected set; }

        [JsonConstructor]
        protected AccountProvider()
        {
        }

        public AccountProvider(long ukprn, long accountId, string accountPublicHashedId, string accountName, int accountProviderId, DateTime created)
        {
            Ukprn = ukprn;
            AccountId = accountId;
            AccountPublicHashedId = accountPublicHashedId;
            AccountName = accountName;
            AccountProviderId = accountProviderId;
            Created = created;
        }
    }
}