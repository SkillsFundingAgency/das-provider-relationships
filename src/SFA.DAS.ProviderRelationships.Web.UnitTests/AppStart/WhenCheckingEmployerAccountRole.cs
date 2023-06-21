using AutoFixture.NUnit3;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Authorization;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.Services.OuterApi;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Authorisation.Handlers;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.AppStart;

public class WhenCheckingEmployerAccountRole
{
    [Test]
    [MoqInlineAutoData(EmployerUserRole.Owner, "Owner", true)]
    [MoqInlineAutoData(EmployerUserRole.Owner, "Transactor", false)]
    [MoqInlineAutoData(EmployerUserRole.Owner, "Viewer", false)]
    [MoqInlineAutoData(EmployerUserRole.Transactor, "Owner", true)]
    [MoqInlineAutoData(EmployerUserRole.Transactor, "Transactor", true)]
    [MoqInlineAutoData(EmployerUserRole.Transactor, "Viewer", false)]
    [MoqInlineAutoData(EmployerUserRole.Viewer, "Owner", true)]
    [MoqInlineAutoData(EmployerUserRole.Viewer, "Transactor", true)]
    [MoqInlineAutoData(EmployerUserRole.Viewer, "Viewer", true)]
    public async Task Then_Checks_Role_And_Returns_True_If_Valid(
        EmployerUserRole userRoleRequired,
        string roleInClaim,
        bool expectedResponse,
        EmployerIdentifier employerIdentifier,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = roleInClaim;
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
        
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId,employerIdentifier.AccountId);
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        
        //Act
        var actual = await authorizationHandler.CheckUserAccountAccess(claimsPrinciple, userRoleRequired);

        //Assert
        actual.Should().Be(expectedResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_Account_Id_In_Url_Returns_False(
        EmployerIdentifier employerIdentifier,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Owner";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
        
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        
        //Act
        var actual = await authorizationHandler.CheckUserAccountAccess(claimsPrinciple, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Not_Valid_Account_Claims_Returns_False(
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(new object()));
        var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
        
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId,"ABC123");
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        
        //Act
        var actual = await authorizationHandler.CheckUserAccountAccess(claimsPrinciple, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_No_Account_Claims_Returns_False(
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        var employerAccounts = new Dictionary<string, EmployerIdentifier>();
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
        
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId,"ABC123");
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        
        //Act
        var actual = await authorizationHandler.CheckUserAccountAccess(claimsPrinciple, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_No_Account_Claims_Matching_Returns_False(
        EmployerIdentifier employerIdentifier,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Owner";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId,"ABC123");
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        
        //Act
        var actual = await authorizationHandler.CheckUserAccountAccess(claimsPrinciple, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_Not_Correct_Account_Claim_Role_Returns_False(
        EmployerIdentifier employerIdentifier,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Owner_Thing";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId,employerIdentifier.AccountId);
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        
        //Act
        var actual = await authorizationHandler.CheckUserAccountAccess(claimsPrinciple, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }
    
    
    [Test, MoqAutoData]
    public async Task Then_If_Not_In_Context_Claims_EmployerAccountService_Checked_And_True_Returned_If_Exists(
        string accountId,
        string userId,
        string email,
        EmployerIdentifier employerIdentifier,
        EmployerUserAccountItem serviceResponse,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        [Frozen] Mock<IUserAccountService> employerAccountService,
        [Frozen] Mock<IOptions<ProviderRelationshipsConfiguration>> configuration,
        EmployerAccountAuthorisationHandler authorizationHandler)
    {
        //Arrange
        configuration.Object.Value.UseGovUkSignIn = false;
        serviceResponse.AccountId = accountId.ToUpper();
        serviceResponse.Role = "Owner";
        employerAccountService.Setup(x => x.GetUserAccounts(userId, email))
            .ReturnsAsync(new EmployerUserAccounts
            {
                EmployerAccounts = new List<EmployerUserAccountItem>{ serviceResponse }
            });
        
        var userClaim = new Claim(EmployerClaims.IdamsUserIdClaimTypeIdentifier, userId);
        var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
        var employerAccountClaim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {employerAccountClaim, userClaim, new Claim(EmployerClaims.IdamsUserEmailClaimTypeIdentifier, email)})});
        var responseMock = new FeatureCollection();
        var httpContext = new DefaultHttpContext(responseMock);
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, accountId.ToUpper());
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        
        //Act
        var actual = await authorizationHandler.CheckUserAccountAccess(claimsPrinciple, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeTrue();
        
    }
}