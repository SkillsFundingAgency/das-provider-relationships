using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Testing.Builders;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.AppStart;

public class WhenPopulatingAccountClaims
{
    [Test, MoqAutoData]
    public async Task Then_The_Claims_Are_Populated_For_Gov_User(
        string nameIdentifier,
        string idamsIdentifier,
        string emailAddress,
        EmployerUserAccounts accountData,
        [Frozen] Mock<IUserAccountService> accountService,
        [Frozen] Mock<IOptions<ProviderRelationshipsConfiguration>> configuration,
        EmployerAccountPostAuthenticationClaimsHandler handler)
    {
        accountData.EmployerUserId = Guid.NewGuid().ToString();
        accountData.IsSuspended = false;
        configuration.Object.Value.UseGovUkSignIn = true;
        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, idamsIdentifier, emailAddress);
        accountService.Setup(x => x.GetUserAccounts(nameIdentifier, emailAddress)).ReturnsAsync(accountData);

        var actual = await handler.GetClaims(tokenValidatedContext);

        accountService.Verify(x => x.GetUserAccounts(nameIdentifier, emailAddress), Times.Once);
        accountService.Verify(x => x.GetUserAccounts(idamsIdentifier, emailAddress), Times.Never);
        actual.Should().ContainSingle(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));
        var actualClaimValue = actual.First(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)).Value;
        JsonConvert.SerializeObject(accountData.EmployerAccounts.ToDictionary(k => k.AccountId)).Should()
            .Be(actualClaimValue);
        actual.First(c => c.Type.Equals(EmployerClaims.IdamsUserIdClaimTypeIdentifier)).Value.Should()
            .Be(accountData.EmployerUserId);
        actual.First(c => c.Type.Equals(EmployerClaims.IdamsUserDisplayNameClaimTypeIdentifier)).Value.Should()
            .Be(accountData.FirstName + " " + accountData.LastName);
        actual.First(c => c.Type.Equals(EmployerClaims.IdamsUserEmailClaimTypeIdentifier)).Value.Should()
            .Be(emailAddress);
        actual.FirstOrDefault(c => c.Type.Equals(ClaimTypes.AuthorizationDecision))?.Value.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_The_Claims_Are_Populated_For_EmployerUsers_User(
        string nameIdentifier,
        string idamsIdentifier,
        string emailAddress,
        EmployerUserAccounts accountData,
        [Frozen] Mock<IUserAccountService> accountService,
        [Frozen] Mock<IOptions<ProviderRelationshipsConfiguration>> configuration,
        EmployerAccountPostAuthenticationClaimsHandler handler)
    {
        accountData.EmployerUserId = Guid.NewGuid().ToString();

        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, idamsIdentifier, emailAddress);
        accountService.Setup(x => x.GetUserAccounts(idamsIdentifier, emailAddress)).ReturnsAsync(accountData);
        configuration.Object.Value.UseGovUkSignIn = false;

        var actual = await handler.GetClaims(tokenValidatedContext);

        accountService.Verify(x => x.GetUserAccounts(nameIdentifier, string.Empty), Times.Never);
        accountService.Verify(x => x.GetUserAccounts(idamsIdentifier, emailAddress), Times.Once);
        actual.Should().ContainSingle(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));

        var actualClaimValue = actual.First(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)).Value;

        JsonConvert.SerializeObject(accountData.EmployerAccounts.ToDictionary(k => k.AccountId)).Should()
            .Be(actualClaimValue);

        actual.FirstOrDefault(c => c.Type.Equals(EmployerClaims.IdamsUserIdClaimTypeIdentifier)).Value.Should()
            .Be(idamsIdentifier);
        actual.First(c => c.Type.Equals(EmployerClaims.IdamsUserEmailClaimTypeIdentifier)).Value.Should()
            .Be(emailAddress);
        actual.FirstOrDefault(c => c.Type.Equals(EmployerClaims.IdamsUserDisplayNameClaimTypeIdentifier)).Should()
            .BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_The_Claims_Are_Populated_For_Gov_User_If_Suspended(
        string nameIdentifier,
        string idamsIdentifier,
        string emailAddress,
        EmployerUserAccounts accountData,
        [Frozen] Mock<IUserAccountService> accountService,
        [Frozen] Mock<IOptions<ProviderRelationshipsConfiguration>> configuration,
        EmployerAccountPostAuthenticationClaimsHandler handler)
    {
        accountData.EmployerUserId = Guid.NewGuid().ToString();
        accountData.IsSuspended = true;
        configuration.Object.Value.UseGovUkSignIn = true;
        var tokenValidatedContext = ArrangeTokenValidatedContext(nameIdentifier, idamsIdentifier, emailAddress);
        accountService.Setup(x => x.GetUserAccounts(nameIdentifier, emailAddress)).ReturnsAsync(accountData);

        var actual = await handler.GetClaims(tokenValidatedContext);

        accountService.Verify(x => x.GetUserAccounts(nameIdentifier, emailAddress), Times.Once);
        accountService.Verify(x => x.GetUserAccounts(idamsIdentifier, emailAddress), Times.Never);
        actual.Should().ContainSingle(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier));
        var actualClaimValue = actual.First(c => c.Type.Equals(EmployerClaims.AccountsClaimsTypeIdentifier)).Value;
        JsonConvert.SerializeObject(accountData.EmployerAccounts.ToDictionary(k => k.AccountId)).Should()
            .Be(actualClaimValue);
        actual.First(c => c.Type.Equals(EmployerClaims.IdamsUserIdClaimTypeIdentifier)).Value.Should()
            .Be(accountData.EmployerUserId);
        actual.First(c => c.Type.Equals(EmployerClaims.IdamsUserDisplayNameClaimTypeIdentifier)).Value.Should()
            .Be(accountData.FirstName + " " + accountData.LastName);
        actual.First(c => c.Type.Equals(EmployerClaims.IdamsUserEmailClaimTypeIdentifier)).Value.Should()
            .Be(emailAddress);
        actual.First(c => c.Type.Equals(ClaimTypes.AuthorizationDecision)).Value.Should().Be("Suspended");
    }

    private static TokenValidatedContext ArrangeTokenValidatedContext(string nameIdentifier, string idamsIdentifier,
        string emailAddress)
    {
        var identity = new ClaimsIdentity(new List<Claim> {
            new(ClaimTypes.NameIdentifier, nameIdentifier),
            new(EmployerClaims.IdamsUserIdClaimTypeIdentifier, idamsIdentifier),
            new(ClaimTypes.Email, emailAddress),
            new(EmployerClaims.IdamsUserEmailClaimTypeIdentifier, emailAddress)
        });

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(identity));

        var httpContext = new DefaultHttpContext();

        return new TokenValidatedContext(httpContext,
            new AuthenticationScheme(",", "", typeof(TestAuthHandler)),
            new OpenIdConnectOptions(), Mock.Of<ClaimsPrincipal>(), new AuthenticationProperties()) {
            Principal = claimsPrincipal
        };
    }


    private class TestAuthHandler : IAuthenticationHandler
    {
        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }
    }
}