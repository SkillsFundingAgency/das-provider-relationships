using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Document.Model
{
    public class CosmosMetaData
    {
        [JsonProperty("schemaVersion")]
        public short? SchemaVersion { get; set; }

        [JsonProperty("schemaType")]
        public string SchemaType { get; set; }
    }
}
