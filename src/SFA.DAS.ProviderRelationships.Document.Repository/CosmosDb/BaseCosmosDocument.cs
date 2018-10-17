using System;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb
{
    public class BaseCosmosDocument
    {
        public BaseCosmosDocument()
        {
        }
        public BaseCosmosDocument(short? schemaVersion, string documentType)
        {
            SchemaVersion = schemaVersion;
            DocumentType = documentType;
        }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("schemaVersion")]
        public short? SchemaVersion { get; set; }

        [JsonProperty("documentType")]
        public string DocumentType { get; set; }

        [JsonIgnore]
        public string ETag { get; set; }

        [JsonProperty("_etag")]
        private string ReadOnlyETag { set { ETag = value; } }
    }
}
