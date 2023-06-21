using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Services.OuterApi;

public class GetUserAccountsResponse
{
    [JsonProperty(PropertyName = "employerUserId")]
    public string EmployerUserId { get; set; }
    [JsonProperty(PropertyName = "firstName")]
    public string FirstName { get; set; }
    [JsonProperty(PropertyName = "lastName")]
    public string LastName { get; set; }
    [JsonProperty(PropertyName = "userAccounts")]
    public List<EmployerIdentifier> UserAccounts { get; set; }
    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; }
    [JsonProperty(PropertyName = "isSuspended")]
    public bool IsSuspended { get; set; }
}