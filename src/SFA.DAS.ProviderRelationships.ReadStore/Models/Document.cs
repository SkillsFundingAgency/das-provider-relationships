using System;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    public abstract class Document
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("metadata")]
        public DocumentMetadata Metadata { get; set; }

        [JsonIgnore]
        public string ETag { get; set; }

        [JsonProperty("_etag")]
        private string ReadOnlyETag { set => ETag = value; }

        protected Document(short schemaVersion, string schemaType)
        {
            Metadata = new DocumentMetadata(schemaVersion, schemaType);
        }
    }
}