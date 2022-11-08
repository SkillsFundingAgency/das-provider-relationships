using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Services.OuterApi;

public class GetUserAccountsResponse
{
    [JsonProperty("UserAccounts")]
    public List<EmployerIdentifier> UserAccounts { get; set; }
}