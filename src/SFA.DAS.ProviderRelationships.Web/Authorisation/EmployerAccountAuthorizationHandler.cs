﻿using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Authorization;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.RouteValues;


namespace SFA.DAS.ProviderRelationships.Web.Authorisation;

public interface IEmployerAccountAuthorisationHandler
{
    Task<bool> IsEmployerAuthorised(AuthorizationHandlerContext context, bool allowAllUserRoles);
    bool CheckUserAccountAccess(ClaimsPrincipal user, EmployerUserRoles userRoleRequired);
}

public class EmployerAccountAuthorisationHandler : IEmployerAccountAuthorisationHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserAccountService _accountsService;
    private readonly ILogger<EmployerAccountAuthorisationHandler> _logger;
    private readonly ProviderRelationshipsConfiguration _configuration;

    public EmployerAccountAuthorisationHandler(IHttpContextAccessor httpContextAccessor, IUserAccountService accountsService, ILogger<EmployerAccountAuthorisationHandler> logger, IOptions<ProviderRelationshipsConfiguration> configuration)
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
        var employerAccountClaim = context.User.FindFirst(c => c.Type.Equals(EmployerClaimTypes.AccountsClaimsTypeIdentifier));

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

        EmployerUserAccountItem employerIdentifier = null;

        if (employerAccounts != null)
        {
            employerIdentifier = employerAccounts.ContainsKey(accountIdFromUrl)
                ? employerAccounts[accountIdFromUrl] : null;
        }

        if (employerAccounts == null || !employerAccounts.ContainsKey(accountIdFromUrl))
        {
            var requiredIdClaim = _configuration.UseGovUkSignIn
                ? ClaimTypes.NameIdentifier : EmployerClaimTypes.IdamsUserIdClaimTypeIdentifier;

            if (!context.User.HasClaim(c => c.Type.Equals(requiredIdClaim)))
                return false;

            var userClaim = context.User.Claims
                .First(c => c.Type.Equals(requiredIdClaim));

            var email = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;

            var userId = userClaim.Value;

            var result = await _accountsService.GetUserAccounts(userId, email);

            var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
            var associatedAccountsClaim = new Claim(EmployerClaimTypes.AccountsClaimsTypeIdentifier, accountsAsJson, JsonClaimValueTypes.Json);

            var updatedEmployerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(associatedAccountsClaim.Value);

            userClaim.Subject.AddClaim(associatedAccountsClaim);

            if (!updatedEmployerAccounts.ContainsKey(accountIdFromUrl))
            {
                return false;
            }
            employerIdentifier = updatedEmployerAccounts[accountIdFromUrl];
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

    public bool CheckUserAccountAccess(ClaimsPrincipal user, EmployerUserRoles userRoleRequired)
    {
        if (!_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValueKeys.AccountHashedId))
        {
            return false;
        }

        Dictionary<string, EmployerUserAccountItem> employerAccounts;
        var accountIdFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues[RouteValueKeys.AccountHashedId].ToString().ToUpper();
        var employerAccountClaim = user.FindFirst(c => c.Type.Equals(EmployerClaimTypes.AccountsClaimsTypeIdentifier));
        try
        {
            employerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(employerAccountClaim.Value);
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

        var employerIdentifier = employerAccounts.ContainsKey(accountIdFromUrl)
            ? employerAccounts[accountIdFromUrl] : null;

        if (employerIdentifier == null)
        {
            return false;
        }

        if (!Enum.TryParse<EmployerUserRoles>(employerIdentifier.Role, true, out var claimUserRole))
        {
            return false;
        }

        switch (userRoleRequired)
        {
            case EmployerUserRoles.Owner when claimUserRole == EmployerUserRoles.Owner:
            case EmployerUserRoles.Transactor when claimUserRole is EmployerUserRoles.Owner or EmployerUserRoles.Transactor:
            case EmployerUserRoles.Viewer when claimUserRole is EmployerUserRoles.Owner or EmployerUserRoles.Transactor or EmployerUserRoles.Viewer:
                return true;
            default:
                return false;
        }
    }

    private static bool CheckUserRoleForAccess(EmployerUserAccountItem employerIdentifier, bool allowAllUserRoles)
    {
        if (!Enum.TryParse<EmployerUserRoles>(employerIdentifier.Role, true, out var userRole))
        {
            return false;
        }

        return allowAllUserRoles || userRole == EmployerUserRoles.Owner;
    }
}