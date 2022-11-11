using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Services.OuterApi
{
    public class EmployerIdentifier
    {
        [JsonProperty("EncodedAccountId")]
        public string AccountId { get; set; }
        [JsonProperty("DasAccountName")]
        public string EmployerName { get; set; }
        [JsonProperty("Role")]
        public string Role { get; set; }
    }
}
