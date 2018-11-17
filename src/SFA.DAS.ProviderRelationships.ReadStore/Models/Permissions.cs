using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class Permissions
    {
        [JsonProperty("operations")]
        public IEnumerable<Operation> Operations { get; protected set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; protected set; }

        [JsonConstructor]
        protected Permissions()
        {
        }

        public Permissions(HashSet<Operation> operations)
        {
            Operations = operations;
        }

        public void UpdateOperations(HashSet<Operation> operations, DateTime updated)
        {
            Operations = operations;
            Updated = updated;
        }
    }
}