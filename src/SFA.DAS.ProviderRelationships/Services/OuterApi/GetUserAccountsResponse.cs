using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Services.OuterApi;

namespace SFA.DAS.ProviderRelationships.Services.OuterApi
{
    public class GetUserAccountsResponse
    {
        [JsonProperty] public List<EmployerIdentifier> UserAccounts { get; set; }
    }
}