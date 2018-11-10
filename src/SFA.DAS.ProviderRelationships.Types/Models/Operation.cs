using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SFA.DAS.ProviderRelationships.Types.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Operation
    {
        CreateCohort = 0
    }
}