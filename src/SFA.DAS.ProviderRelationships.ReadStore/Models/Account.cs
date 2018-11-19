using System;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class Account
    {
        [JsonProperty("id")]
        public long Id { get; protected set; }

        [JsonProperty("accountPublicHashedId")]
        public string AccountPublicHashedId { get; protected set; }

        [JsonProperty("accountName")]
        public string AccountName { get; protected set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; protected set; }

        [JsonConstructor]
        protected Account()
        {
        }

        public Account(long id, string accountPublicHashedId, string accountName)
        {
            Id = id;
            AccountPublicHashedId = accountPublicHashedId;
            AccountName = accountName;
        }
    }
}