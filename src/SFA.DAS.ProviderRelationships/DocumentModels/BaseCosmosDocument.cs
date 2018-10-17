using System;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.DocumentModels
{
    public class BaseCosmosDocument
    {
        public BaseCosmosDocument()
        {
        }
        public BaseCosmosDocument(short? schemaVersion, string schemaType)
        {
            MetaData = new CosmosMetaData
            {
                SchemaVersion = schemaVersion,
                SchemaType = schemaType
            };
        }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("metaData")]
        public CosmosMetaData MetaData { get; set; }

        [JsonIgnore]
        public string ETag { get; set; }

        [JsonProperty("_etag")]
        private string ReadOnlyETag { set { ETag = value; } }
    }
}
