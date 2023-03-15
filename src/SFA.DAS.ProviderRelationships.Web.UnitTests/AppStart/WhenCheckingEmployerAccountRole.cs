using AutoFixture.NUnit3;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Authorization;
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
    public void Then_Checks_Role_And_Returns_True_If_Valid(
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
        var actual = authorizationHandler.CheckUserAccountAccess(claimsPrinciple, userRoleRequired);

        //Assert
        actual.Should().Be(expectedResponse);
    }

    [Test, MoqAutoData]
    public void Then_If_No_Account_Id_In_Url_Returns_False(
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
        var actual = authorizationHandler.CheckUserAccountAccess(claimsPrinciple, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_Not_Valid_Account_Claims_Returns_False(
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
        var actual = authorizationHandler.CheckUserAccountAccess(claimsPrinciple, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_No_Account_Claims_Returns_False(
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
        var actual = authorizationHandler.CheckUserAccountAccess(claimsPrinciple, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_No_Account_Claims_Matching_Returns_False(
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
        var actual = authorizationHandler.CheckUserAccountAccess(claimsPrinciple, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }
    
    [Test, MoqAutoData]
    public void Then_If_Not_Correct_Account_Claim_Role_Returns_False(
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
        var actual = authorizationHandler.CheckUserAccountAccess(claimsPrinciple, EmployerUserRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }
}