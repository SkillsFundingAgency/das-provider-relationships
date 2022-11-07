using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.Services;

public interface IOuterApiClient
{
    Task<TResponse> Get<TResponse>(IGetApiRequest request);
}

public interface IGetApiRequest
{
    string GetUrl { get; }
}

public class GetEmployerAccountRequest : IGetApiRequest
{
    private readonly string _userId;
    private readonly string _email;

    public GetEmployerAccountRequest(string userId, string email)
    {
        _userId = userId;
        _email = HttpUtility.UrlEncode(email);
    }

    public string GetUrl => $"accountusers/{_userId}/accounts?email={_email}";
}

public class GetUserAccountsResponse
{
    [JsonProperty("UserAccounts")]
    public List<EmployerIdentifier> UserAccounts { get; set; }
}

public class EmployerIdentifier
{
    [JsonProperty("EncodedAccountId")]
    public string AccountId { get; set; }
    [JsonProperty("DasAccountName")]
    public string EmployerName { get; set; }
    [JsonProperty("Role")]
    public string Role { get; set; }
}