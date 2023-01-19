using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Context;
using SFA.DAS.Authorization.Handlers;
using SFA.DAS.Authorization.Results;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.ProviderRelationships.Web.Authorization;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Authorization
{
    [TestFixture]
    [Parallelizable]
    public class LocalAuthorizationHandlerTests : FluentTest<LocalAuthorizationHandlerTestsFixture>
    {
        [Test]
        public Task GetAuthorizationResult_WhenEnvironmentIsLocal_ThenShouldReturnAuthorizedAuthorizationResult()
        {
            return TestAsync(f => f.SetEnvironment(DasEnv.LOCAL), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.BeOfType<AuthorizationResult>().Which.IsAuthorized.Should().BeTrue());
        }
        
        [Test]
        public Task GetAuthorizationResult_WhenEnvironmentIsNotLocal_ThenShouldReturnAuthorizationResult()
        {
            return TestAsync(f => f.SetEnvironment(DasEnv.PROD).SetAuthorizationResult(), f => f.GetAuthorizationResult(), (f, r) => r.Should().NotBeNull().And.BeSameAs(f.AuthorizationResult));
        }
    }

    public class LocalAuthorizationHandlerTestsFixture
    {
        public IReadOnlyCollection<string> Options { get; set; }
        public IAuthorizationContext AuthorizationContext { get; set; }
        public IAuthorizationHandler AuthorizationHandlerDecorator { get; set; }
        public Mock<IAuthorizationHandler> AuthorizationHandler { get; set; }
        public Mock<IEnvironmentService> EnvironmentService { get; set; }
        public AuthorizationResult AuthorizationResult { get; set; }

        public LocalAuthorizationHandlerTestsFixture()
        {
            Options = new [] { "" };
            AuthorizationContext = new AuthorizationContext();
            AuthorizationHandler = new Mock<IAuthorizationHandler>();
            EnvironmentService = new Mock<IEnvironmentService>();
            AuthorizationHandlerDecorator = new LocalAuthorizationHandler(AuthorizationHandler.Object, EnvironmentService.Object);
            AuthorizationResult = new AuthorizationResult();
        }

        public Task<AuthorizationResult> GetAuthorizationResult()
        {
            return AuthorizationHandlerDecorator.GetAuthorizationResult(Options, AuthorizationContext);
        }

        public LocalAuthorizationHandlerTestsFixture SetEnvironment(DasEnv environment)
        {
            EnvironmentService.Setup(e => e.IsCurrent(environment)).Returns(true);
            
            return this;
        }

        public LocalAuthorizationHandlerTestsFixture SetAuthorizationResult()
        {
            AuthorizationHandler.Setup(h => h.GetAuthorizationResult(Options, AuthorizationContext)).ReturnsAsync(AuthorizationResult);
            
            return this;
        }
    }
}