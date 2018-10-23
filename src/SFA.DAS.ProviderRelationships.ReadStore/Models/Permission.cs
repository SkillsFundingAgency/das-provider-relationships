using System;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    public class Permission
    {
        [JsonProperty("permission")]
        public PermissionEnumDto GrantPermission { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }
    }
}


