using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Authorization;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.RouteValues;


namespace SFA.DAS.ProviderRelationships.Web.Authorisation
{
    public class EmployerAccountAuthorizationHandler : IEmployerAccountAuthorisationHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployerAccountService _accountsService;
        private readonly ILogger<EmployerAccountAuthorizationHandler> _logger;
        private readonly ProviderRelationshipsConfiguration _config;

        public EmployerAccountAuthorizationHandler(
            IHttpContextAccessor httpContextAccessor, 
            IEmployerAccountService accountsService, 
            ILogger<EmployerAccountAuthorizationHandler> logger, 
            IOptions<ProviderRelationshipsConfiguration> configOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _accountsService = accountsService;
            _logger = logger;
            _config = configOptions.Value;
        }
    
        public bool IsEmployerAuthorised(AuthorizationHandlerContext context, EmployerUserRole allowedRole)
        {
            if (!_httpContextAccessor.HttpContext.Request.RouteValues.ContainsKey(RouteValueKeys.AccountHashedId))
            {
                return false;
            }
            
            var accountIdFromUrl = _httpContextAccessor.HttpContext.Request.RouteValues[RouteValueKeys.AccountHashedId].ToString().ToUpper();
            var employerAccountClaim = context.User.FindFirst(c=>c.Type.Equals(EmployerClaimTypes.AssociatedAccounts));

            if(employerAccountClaim?.Value == null)
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
                var requiredIdClaim =_config.UseGovSignIn 
                    ? ClaimTypes.NameIdentifier : EmployerClaimTypes.UserId;
                
                if (!context.User.HasClaim(c => c.Type.Equals(requiredIdClaim)))
                    return false;
                
                var userClaim = context.User.Claims
                    .First(c => c.Type.Equals(requiredIdClaim));

                var email = context.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;

                var userId = userClaim.Value;

                var result = _accountsService.GetUserAccounts(userId, email).Result;
                
                var accountsAsJson = JsonConvert.SerializeObject(result.EmployerAccounts.ToDictionary(k => k.AccountId));
                var associatedAccountsClaim = new Claim(EmployerClaimTypes.AssociatedAccounts, accountsAsJson, JsonClaimValueTypes.Json);
                
                var updatedEmployerAccounts = JsonConvert.DeserializeObject<Dictionary<string, EmployerUserAccountItem>>(associatedAccountsClaim.Value);

                userClaim.Subject.AddClaim(associatedAccountsClaim);
                
                if (!updatedEmployerAccounts.ContainsKey(accountIdFromUrl))
                {
                    return false;
                }

                employerIdentifier = updatedEmployerAccounts[accountIdFromUrl];
            }

            if (!_httpContextAccessor.HttpContext.Items.ContainsKey(ContextItemKeys.EmployerIdentifier))
            {
                _httpContextAccessor.HttpContext.Items.Add(ContextItemKeys.EmployerIdentifier, employerAccounts.GetValueOrDefault(accountIdFromUrl));
            }

            if (!CheckUserRoleForAccess(employerIdentifier, allowedRole))
            {
                return false;
            }
            
            return true;
        }
        
        private static bool CheckUserRoleForAccess(EmployerUserAccountItem employerIdentifier, EmployerUserRole allowedRole)
        {
            if (!Enum.TryParse<EmployerUserRole>(employerIdentifier.Role, true, out var userRole))
            {
                return false;
            }

            return userRole == allowedRole;
        }
    }

    public interface IEmployerAccountService
    {
        Task<EmployerUserAccounts> GetUserAccounts(string userId, string email);
    }

    public class EmployerUserAccounts
    {
        public IEnumerable<EmployerUserAccountItem> EmployerAccounts { get ; set ; }

        public static implicit operator EmployerUserAccounts(GetUserAccountsResponse source)
        {
            if (source?.UserAccounts == null)
            {
                return new EmployerUserAccounts
                {
                    EmployerAccounts = new List<EmployerUserAccountItem>()
                };
            }
            
            return new EmployerUserAccounts
            {
                EmployerAccounts = source.UserAccounts.Select(c=>(EmployerUserAccountItem)c).ToList()
            };
        }
    }
    
    public class EmployerUserAccountItem
    {
        public string AccountId { get; set; }
        public string EmployerName { get; set; }
        public string Role { get; set; }
        
        public static implicit operator EmployerUserAccountItem(EmployerIdentifier source)
        {
            return new EmployerUserAccountItem
            {
                AccountId = source.AccountId,
                EmployerName = source.EmployerName,
                Role = source.Role
            };
        }
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

    public enum EmployerUserRole
    {
        None = 0,
        Owner = 1,
        Transactor = 2,
        Viewer = 3
    }
}