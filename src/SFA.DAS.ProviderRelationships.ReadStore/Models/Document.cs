using System;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Document.Repository;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal abstract class Document : IDocumentEntity
    {
        [JsonProperty("id")]
        public virtual Guid? Id { get; protected set; }

        [JsonIgnore]
        public virtual string ETag { get; protected set; }

        [JsonProperty("_etag")]
        private string ReadOnlyETag { set => ETag = value; }

        [JsonProperty("metadata")]
        public virtual DocumentMetadata Metadata { get; protected set;}

        protected Document(short schemaVersion, string schemaType)
        {
            Metadata = new DocumentMetadata(schemaVersion, schemaType);
        }
    }
}