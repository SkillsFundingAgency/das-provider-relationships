using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    public class DocumentMetaData
    {
        [JsonProperty("schemaVersion")]
        public short? SchemaVersion { get; set; }

        [JsonProperty("schemaType")]
        public string SchemaType { get; set; }
    }
}
