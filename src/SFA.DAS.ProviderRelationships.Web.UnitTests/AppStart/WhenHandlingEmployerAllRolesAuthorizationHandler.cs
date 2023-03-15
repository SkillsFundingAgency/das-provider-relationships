using AutoFixture.NUnit3;
using SFA.DAS.ProviderRelationships.Web.Authorisation.Handlers;
using SFA.DAS.ProviderRelationships.Web.Authorisation.Requirements;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.AppStart;

public class WhenHandlingEmployerAllRolesAuthorizationHandler
{
    [Test, MoqAutoData]
    public async Task Then_Returns_Succeeded_If_Employer_Is_Authorized_For_Any_Role(
        EmployerAllRolesRequirement requirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        [Frozen] Mock<IEmployerAccountAuthorisationHandler> handler,
        EmployerAllRolesAuthorizationHandler authorizationHandler)
    {
        //Arrange
        var context = new AuthorizationHandlerContext(new [] {requirement}, new ClaimsPrincipal(), null);
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        handler.Setup(x => x.IsEmployerAuthorised(context, true)).ReturnsAsync(true);
        
        //Act
        await authorizationHandler.HandleAsync(context);

        //Assert
        context.HasSucceeded.Should().BeTrue();
    }
    [Test, MoqAutoData]
    public async Task Then_Returns_Failed_If_Employer_Is_Not_Authorized_For_Any_Role(
        EmployerAllRolesRequirement requirement,
        [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
        [Frozen] Mock<IEmployerAccountAuthorisationHandler> handler,
        EmployerAllRolesAuthorizationHandler authorizationHandler)
    {
        //Arrange
        var context = new AuthorizationHandlerContext(new [] {requirement}, new ClaimsPrincipal(), null);
        var httpContext = new DefaultHttpContext(new FeatureCollection());
        httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
        handler.Setup(x => x.IsEmployerAuthorised(context, true)).ReturnsAsync(false);
        
        //Act
        await authorizationHandler.HandleAsync(context);

        //Assert
        context.HasSucceeded.Should().BeFalse();
    }
}