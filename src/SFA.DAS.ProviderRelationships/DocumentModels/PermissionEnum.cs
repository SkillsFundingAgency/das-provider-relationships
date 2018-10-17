using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SFA.DAS.ProviderRelationships.DocumentModels
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PermissionEnum{
        CreateCohort,
        Another
    }

}


