using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderRelationships.Web.Authorization;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Authorization
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationHandlerDecoratorTests : FluentTest<AuthorizationHandlerDecoratorTestsFixture>
    {
        [Test]
        public Task GetAuthorizationResult_WhenEnvironmentIsLocal_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return RunAsync(f => f.SetEnvironment(DasEnv.LOCAL), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.BeOfType<AuthorizationResult>().Which.IsAuthorized.Should().BeTrue());
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenEnvironmentIsNotLocal_ThenShouldReturnAuthorizationResult()
        {
            return RunAsync(f => f.SetEnvironment(DasEnv.PROD).SetAuthorizationResult(), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.BeSameAs(f.AuthorizationResult));
        }
    }

    public class AuthorizationHandlerDecoratorTestsFixture
    {
        public IReadOnlyCollection<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler AuthorizationHandlerDecorator { get; set; }
        public Mock<IAuthorizationHandler> AuthorizationHandler { get; set; }
        public Mock<IEnvironmentService> EnvironmentService { get; set; }
        public AuthorizationResult AuthorizationResult { get; set; }

        public AuthorizationHandlerDecoratorTestsFixture()
        {
            Options = new [] { "" };
            AuthorizationContext = new AuthorizationContext();
            AuthorizationHandler = new Mock<IAuthorizationHandler>();
            EnvironmentService = new Mock<IEnvironmentService>();
            AuthorizationHandlerDecorator = new AuthorizationHandlerDecorator(AuthorizationHandler.Object, EnvironmentService.Object);
            AuthorizationResult = new AuthorizationResult();
        }

        public Task<AuthorizationResult> GetAuthorizationResult()
        {
            return AuthorizationHandlerDecorator.GetAuthorizationResult(Options, AuthorizationContext);
        }

        public AuthorizationHandlerDecoratorTestsFixture SetEnvironment(DasEnv environment)
        {
            EnvironmentService.Setup(e => e.IsCurrent(environment)).Returns(true);
            
            return this;
        }

        public AuthorizationHandlerDecoratorTestsFixture SetAuthorizationResult()
        {
            AuthorizationHandler.Setup(h => h.GetAuthorizationResult(Options, AuthorizationContext)).ReturnsAsync(AuthorizationResult);
            
            return this;
        }
    }
}