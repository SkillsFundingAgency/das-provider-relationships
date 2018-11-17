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

        [JsonProperty("created")]
        public DateTime Created { get; protected set; }

        [JsonProperty("deleted")]
        public DateTime? Deleted { get; protected set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; protected set; }

        [JsonConstructor]
        protected Permissions()
        {
        }

        public Permissions(HashSet<Operation> operations, DateTime created)
        {
            Operations = operations;
            Created = created;
        }
    }
}