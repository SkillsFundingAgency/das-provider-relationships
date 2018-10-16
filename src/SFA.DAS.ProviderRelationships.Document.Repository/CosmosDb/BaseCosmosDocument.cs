using System;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public class BaseCosmosDocument
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonIgnore]
        public string ETag { get; set; }

        [JsonProperty("_etag")]
        private string etag { set { ETag = value; } }
    }
}
