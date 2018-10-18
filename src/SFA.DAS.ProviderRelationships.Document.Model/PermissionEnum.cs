using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SFA.DAS.ProviderRelationships.Document.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PermissionEnum{
        CreateCohort,
        Another
    }

}


