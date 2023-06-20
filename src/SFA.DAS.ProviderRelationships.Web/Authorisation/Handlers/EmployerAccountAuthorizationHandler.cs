using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Authorization;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.RouteValues;

namespace SFA.DAS.ProviderRelationships.Web.Authorisation.Handlers;

public interface IEmployerAccountAuthorisationHandler
{
    Task<bool> IsEmployerAuthorised(AuthorizationHandlerContext context, bool allowAllUserRoles);
    Task<bool> CheckUserAccountAccess(ClaimsPrincipal user, EmployerUserRole userRoleRequired);
}

public class EmployerAccountAuthorisationHandler : IEmployerAccountAuthorisationHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserAccountService _accountsService;
    private readonly ILogger<EmployerAccountAuthorisationHandler> _logger;
    private readonly ProviderRelationshipsConfiguration _configuration;

    public EmployerAccountAuthorisationHandler(
        IHttpContextAccessor httpContextAccessor,
        IUserAccountService accountsService, 
        ILogger<EmployerAccountAuthorisationHandler> logger, 
        IOptions<ProviderRelationshipsConfiguration> configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _accountsService = accountsService;
        _logger = logger;
        _configuration = configuration.Value;
    }

    public async Task<bool> IsEmployerAuthorised(AuthorizationHandlerContext context, bool allowAllUserRoles)
    {
        if (!_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValueKeys.AccountHashedId))
        {
            return false;
        }
        var accountIdFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues[RouteValueKeys.AccountHashedId].ToString().ToUpper();
        var employerAccountClaim = context.User.FindFirst(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));

        if (employerAccountClaim?.Value == null)
            return false;

        Dictionary<string, EmployerUserAccountItem> employerAccounts;

        try
        {
            employerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(employerAccountClaim.Value);
        }
        catch (JsonSerializationException e)
        {
            _logger.LogError(e, "Could not deserialize employer account claim for user");
            return false;
        }

        EmployerUserAccountItem employerIdentifier = await GetAccountIdentifier(employerAccounts, context.User, accountIdFromUrl);

        if (employerIdentifier == null)
        {
            return false;
        }
        
        if (!_httpContextAccessor.HttpContext.Items.ContainsKey("Employer"))
        {
            _httpContextAccessor.HttpContext.Items.Add("Employer", employerAccounts.GetValueOrDefault(accountIdFromUrl));
        }

        if (!CheckUserRoleForAccess(employerIdentifier, allowAllUserRoles))
        {
            return false;
        }

        return true;
    }

    public async Task<bool> CheckUserAccountAccess(ClaimsPrincipal user, EmployerUserRole userRoleRequired)
    {
        if (!_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValueKeys.AccountHashedId))
        {
            return false;
        }

        Dictionary<string, EmployerUserAccountItem> employerAccounts = null;
        var accountIdFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues[RouteValueKeys.AccountHashedId].ToString().ToUpper();
        var employerAccountClaim = user.FindFirst(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));
        try
        {
            if (employerAccountClaim != null)
            {
                employerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(employerAccountClaim.Value);    
            }
        }
        catch (JsonSerializationException e)
        {
            _logger.LogError(e, "Could not deserialize employer account claim for user");
            return false;
        }

        if (employerAccounts == null)
        {
            return false;
        }

        var employerIdentifier = await GetAccountIdentifier(employerAccounts, user, accountIdFromUrl);

        if (employerIdentifier == null)
        {
            return false;
        }

        if (!Enum.TryParse<EmployerUserRole>(employerIdentifier.Role, true, out var claimUserRole))
        {
            return false;
        }

        switch (userRoleRequired)
        {
            case EmployerUserRole.Owner when claimUserRole == EmployerUserRole.Owner:
            case EmployerUserRole.Transactor when claimUserRole is EmployerUserRole.Owner or EmployerUserRole.Transactor:
            case EmployerUserRole.Viewer when claimUserRole is EmployerUserRole.Owner or EmployerUserRole.Transactor or EmployerUserRole.Viewer:
                return true;
            default:
                return false;
        }
    }

    private static bool CheckUserRoleForAccess(EmployerUserAccountItem employerIdentifier, bool allowAllUserRoles)
    {
        if (!Enum.TryParse<EmployerUserRole>(employerIdentifier.Role, true, out var userRole))
        {
            return false;
        }

        return allowAllUserRoles || userRole == EmployerUserRole.Owner;
    }

    private async Task<EmployerUserAccountItem> GetAccountIdentifier(Dictionary<string, EmployerUserAccountItem> employerAccounts, ClaimsPrincipal user, string accountId )
    {
        if (employerAccounts == null || !employerAccounts.ContainsKey(accountId))
        {
            var requiredIdClaim = _configuration.UseGovUkSignIn
                ? ClaimTypes.NameIdentifier : EmployerClaims.IdamsUserIdClaimTypeIdentifier;

            if (!user.HasClaim(c => c.Type.Equals(requiredIdClaim)))
            {
                return null;
            }

            var userClaim = user.Claims.First(c => c.Type.Equals(requiredIdClaim));

            var email = user.Claims.FirstOrDefault(c => c.Type.Equals(EmployerClaims.IdamsUserEmailClaimTypeIdentifier))?.Value;

            var userId = userClaim.Value;

            var result = await _accountsService.GetUserAccounts(userId, email);

            var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
            var associatedAccountsClaim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json);

            var updatedEmployerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(associatedAccountsClaim.Value);

            userClaim.Subject.AddClaim(associatedAccountsClaim);

            if (!updatedEmployerAccounts.ContainsKey(accountId))
            {
                return null;
            }

            return updatedEmployerAccounts[accountId];
        }
        
        return employerAccounts.ContainsKey(accountId)
            ? employerAccounts[accountId] : null;
    }
}