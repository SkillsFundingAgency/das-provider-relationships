using System;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    public class Document
    {
        public Document()
        {
        }
        
        public Document(short? schemaVersion, string schemaType)
        {
            MetaData = new DocumentMetaData
            {
                SchemaVersion = schemaVersion,
                SchemaType = schemaType
            };
        }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("metaData")]
        public DocumentMetaData MetaData { get; set; }

        [JsonIgnore]
        public string ETag { get; set; }

        [JsonProperty("_etag")]
        private string ReadOnlyETag { set { ETag = value; } }
    }
}