using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Web.Authorisation;

namespace SFA.DAS.ProviderRelationships.Web.Authentication
{
    public class EmployerStubAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        
        public EmployerStubAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var accountClaims = new Dictionary<string, EmployerUserAccountItem>();
            accountClaims.Add("", new EmployerUserAccountItem
            {
                Role = "Owner",
                AccountId = "ABC123",
                EmployerName = "Stub Employer"
            });
            
            var claims = new[]
            {
                new Claim(EmployerClaimTypes.AssociatedAccounts, JsonConvert.SerializeObject(accountClaims)),
                new Claim(EmployerClaimTypes.EmailAddress, "testemployer@user.com"),
                new Claim(EmployerClaimTypes.UserId, Guid.NewGuid().ToString())
            };
            var identity = new ClaimsIdentity(claims, "Employer-stub");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Employer-stub");
 
            var result = AuthenticateResult.Success(ticket);
 
            return Task.FromResult(result);
        }
    }
}