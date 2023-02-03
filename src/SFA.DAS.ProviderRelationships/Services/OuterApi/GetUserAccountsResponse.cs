using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Services.OuterApi;

public class GetUserAccountsResponse
{
    [JsonProperty]
    public string EmployerUserId { get; set; }
    [JsonProperty]
    public string FirstName { get; set; }
    [JsonProperty]
    public string LastName { get; set; }
    [JsonProperty] public List<EmployerIdentifier> UserAccounts { get; set; }
}