using System;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    public class GrantPermission
    {
        [JsonProperty("permission")]
        public PermissionEnumDto Permission { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("messageId")]
        public string MessageId { get; set; }
    }
}


