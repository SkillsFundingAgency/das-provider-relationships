using System.Collections.Generic;
using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Services.OuterApi;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Authorisation;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.Testing.AutoFixture;
using EmployerIdentifier = SFA.DAS.ProviderRelationships.Web.Authorisation.EmployerIdentifier;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Authorisation;

public class WhenCheckingEmployerAccountRole
{
    [Test]
    [MoqInlineAutoData(EmployerUserAuthorisationRole.Owner, "Owner", true)]
    [MoqInlineAutoData(EmployerUserAuthorisationRole.Owner, "Transactor", false)]
    [MoqInlineAutoData(EmployerUserAuthorisationRole.Owner, "Viewer", false)]
    [MoqInlineAutoData(EmployerUserAuthorisationRole.Transactor, "Owner", true)]
    [MoqInlineAutoData(EmployerUserAuthorisationRole.Transactor, "Transactor", true)]
    [MoqInlineAutoData(EmployerUserAuthorisationRole.Transactor, "Viewer", false)]
    [MoqInlineAutoData(EmployerUserAuthorisationRole.Viewer, "Owner", true)]
    [MoqInlineAutoData(EmployerUserAuthorisationRole.Viewer, "Transactor", true)]
    [MoqInlineAutoData(EmployerUserAuthorisationRole.Viewer, "Viewer", true)]
    public void Then_Checks_Role_And_Returns_True_If_Valid(
        EmployerUserAuthorisationRole userRoleRequired,
        string roleInClaim,
        bool expectedResponse,
        EmployerIdentifier employerIdentifier,
        EmployerAccountOwnerRequirement ownerRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorizationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = roleInClaim;
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier>{{employerIdentifier.AccountId, employerIdentifier}};
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[] {claim})});
        var context = new AuthorizationHandlerContext(new[] { ownerRequirement }, claimsPrinciple, null);
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, employerIdentifier.AccountId);
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = authorizationHandler.IsEmployerAuthorised(context, userRoleRequired);

        //Assert
        actual.Should().Be(expectedResponse);
    }

    [Test, MoqAutoData]
    public void Then_If_No_Account_Id_In_Url_Returns_False(
        EmployerIdentifier employerIdentifier,
        EmployerAccountOwnerRequirement ownerRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorizationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Owner";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier> { { employerIdentifier.AccountId, employerIdentifier } };
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
        var context = new AuthorizationHandlerContext(new[] { ownerRequirement }, claimsPrinciple, null);
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = authorizationHandler.IsEmployerAuthorised(context, EmployerUserAuthorisationRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public void Then_If_Not_Valid_Account_Claims_Returns_False(
        EmployerAccountOwnerRequirement ownerRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorizationHandler authorizationHandler)
    {
        //Arrange
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(new object()));
        var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
        var context = new AuthorizationHandlerContext(new[] { ownerRequirement }, claimsPrinciple, null);
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, "ABC123");
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = authorizationHandler.IsEmployerAuthorised(context, EmployerUserAuthorisationRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public void Then_If_No_Account_Claims_Returns_False(
        EmployerAccountOwnerRequirement ownerRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorizationHandler authorizationHandler)
    {
        //Arrange
        var employerAccounts = new Dictionary<string, EmployerIdentifier>();
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
        var context = new AuthorizationHandlerContext(new[] { ownerRequirement }, claimsPrinciple, null);
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, "ABC123");
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = authorizationHandler.IsEmployerAuthorised(context, EmployerUserAuthorisationRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public void Then_If_No_Account_Claims_Matching_Returns_False(
        EmployerIdentifier employerIdentifier,
        EmployerAccountOwnerRequirement ownerRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorizationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Owner";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier> { { employerIdentifier.AccountId, employerIdentifier } };
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
        var context = new AuthorizationHandlerContext(new[] { ownerRequirement }, claimsPrinciple, null);
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, "ABC123");
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = authorizationHandler.IsEmployerAuthorised(context, EmployerUserAuthorisationRole.Viewer);
        
        //Assert
        actual.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public void Then_If_Not_Correct_Account_Claim_Role_Returns_False(
        EmployerIdentifier employerIdentifier,
        EmployerAccountOwnerRequirement ownerRequirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        EmployerAccountAuthorizationHandler authorizationHandler)
    {
        //Arrange
        employerIdentifier.Role = "Owner_Thing";
        employerIdentifier.AccountId = employerIdentifier.AccountId.ToUpper();
        var employerAccounts = new Dictionary<string, EmployerIdentifier> { { employerIdentifier.AccountId, employerIdentifier } };
        var claim = new Claim(EmployerClaims.AccountsClaimsTypeIdentifier, JsonConvert.SerializeObject(employerAccounts));
        var claimsPrinciple = new ClaimsPrincipal(new[] { new ClaimsIdentity(new[] { claim }) });
        var context = new AuthorizationHandlerContext(new[] { ownerRequirement }, claimsPrinciple, null);
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContext.Request.RouteValues.Add(RouteValueKeys.AccountHashedId, employerIdentifier.AccountId);
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        //Act
        var actual = authorizationHandler.IsEmployerAuthorised(context, EmployerUserAuthorisationRole.Viewer);

        //Assert
        actual.Should().BeFalse();
    }
}