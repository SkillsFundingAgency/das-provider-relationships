using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class AccountProvider
    {
        [JsonProperty("id")]
        public int Id { get; protected set; }

        [JsonProperty("operations")]
        public IEnumerable<Operation> Operations { get; protected set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; protected set; }

        [JsonConstructor]
        protected AccountProvider()
        {
        }

        public AccountProvider(int id, HashSet<Operation> operations)
        {
            Id = id;
            Operations = operations;
        }

        public void UpdateOperations(HashSet<Operation> operations, DateTime updated)
        {
            Operations = operations;
            Updated = updated;
        }
    }
}