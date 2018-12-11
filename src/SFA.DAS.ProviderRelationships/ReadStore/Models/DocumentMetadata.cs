using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    public class DocumentMetadata
    {
        [JsonProperty("schemaType")]
        public string SchemaType { get; }

        [JsonProperty("schemaVersion")]
        public short SchemaVersion { get; }

        public DocumentMetadata(string schemaType, short schemaVersion)
        {
            SchemaType = schemaType;
            SchemaVersion = schemaVersion;
        }
    }
}
