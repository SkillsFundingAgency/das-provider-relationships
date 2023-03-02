using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Services.OuterApi;

namespace SFA.DAS.ProviderRelationships.Services;

public interface IUserAccountService
{
    Task<EmployerUserAccounts> GetUserAccounts(string userId, string email);
}

public class UserAccountService : IUserAccountService
{
    private readonly IOuterApiClient _outerApiClient;

    public UserAccountService(IOuterApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }
    public async Task<EmployerUserAccounts> GetUserAccounts(string userId, string email)
    {
        var actual = await _outerApiClient.Get<GetUserAccountsResponse>(new GetEmployerAccountRequest(email, userId));

        return actual;
    }
}
