using System;
using System.Web;
using System.Web.Routing;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.HashingService;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Web.Authorization;
using SFA.DAS.ProviderRelationships.Web.Routing;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Authorization
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationContextProviderTests : FluentTest<AuthorizationContextProviderTestsFixture>
    {
        [Test]
        public void GetAuthorizationContext_WhenGettingAuthorizationContextAndAccountIdExistsAndIsValidAndUserIsAuthenticatedAndUserRefIsValid_ThenShouldReturnAuthroizationContextWithAccountIdAndUserRefValues()
        {
            Run(f => f.SetValidAccountId().SetValidUserRef(), f => f.GetAuthorizationContext(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Get<string>("AccountHashedId").Should().Be(f.AccountHashedId);
                r.Get<long?>("AccountId").Should().Be(f.AccountId);
                r.Get<Guid?>("UserRef").Should().Be(f.UserRef);
            });
        }
        
        [Test]
        public void GetAuthorizationContext_WhenGettingAuthorizationContextAndAccountIdDoesNotExistAndUserIsNotAuthenticated_ThenShouldReturnAuthroizationContextWithoutAccountIdAndUserRefValues()
        {
            Run(f => f.SetNoAccountId().SetUnauthenticatedUser(), f => f.GetAuthorizationContext(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Get<string>("AccountHashedId").Should().BeNull();
                r.Get<long?>("AccountId").Should().BeNull();
                r.Get<Guid?>("UserRef").Should().BeNull();
            });
        }
        
        [Test]
        public void GetAuthorizationContext_WhenGettingAuthorizationContextAndAccountIdExistsAndIsInvalid_ThenShouldThrowUnauthorizedAccessException()
        {
            Run(f => f.SetInvalidAccountId(), f => f.GetAuthorizationContext(), (f, r) => r.Should().Throw<UnauthorizedAccessException>());
        }
        
        [Test]
        public void GetAuthorizationContext_WhenGettingAuthorizationContextAndUserIsAuthenticatedAndUserRefIsInvalid_ThenShouldThrowUnauthorizedAccessException()
        {
            Run(f => f.SetInvalidAccountId().SetInvalidUserRef(), f => f.GetAuthorizationContext(), (f, r) => r.Should().Throw<UnauthorizedAccessException>());
        }
    }

    public class AuthorizationContextProviderTestsFixture
    {
        public IAuthorizationContextProvider AuthorizationContextProvider { get; set; }
        public Mock<HttpContextBase> HttpContext { get; set; }
        public Mock<IHashingService> HashingService { get; set; }
        public Mock<IAuthenticationService> AuthenticationService { get; set; }
        public string AccountHashedId { get; set; }
        public long AccountId { get; set; }
        public Guid UserRef { get; set; }
        public string UserRefClaimValue { get; set; }

        public AuthorizationContextProviderTestsFixture()
        {
            HttpContext = new Mock<HttpContextBase>();
            HashingService = new Mock<IHashingService>();
            AuthenticationService = new Mock<IAuthenticationService>();
            AuthorizationContextProvider = new AuthorizationContextProvider(HttpContext.Object, HashingService.Object, AuthenticationService.Object);
        }

        public IAuthorizationContext GetAuthorizationContext()
        {
            return AuthorizationContextProvider.GetAuthorizationContext();
        }

        public AuthorizationContextProviderTestsFixture SetValidAccountId()
        {
            AccountHashedId = "ABC";
            AccountId = 123;

            var routeData = new RouteData();
            
            routeData.Values[RouteDataKeys.AccountHashedId] = AccountHashedId;
            
            HttpContext.Setup(c => c.Request.RequestContext.RouteData).Returns(routeData);
            HashingService.Setup(h => h.DecodeValue(AccountHashedId)).Returns(AccountId);
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidAccountId()
        {
            AccountHashedId = "AAA";

            var routeData = new RouteData();
            
            routeData.Values[RouteDataKeys.AccountHashedId] = AccountHashedId;
            
            HttpContext.Setup(c => c.Request.RequestContext.RouteData).Returns(routeData);
            HashingService.Setup(h => h.DecodeValue(AccountHashedId)).Throws<Exception>();
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetNoAccountId()
        {
            HttpContext.Setup(c => c.Request.RequestContext.RouteData).Returns(new RouteData());
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetValidUserRef()
        {
            UserRef = Guid.NewGuid();
            UserRefClaimValue = UserRef.ToString();
            
            var userRefClaimValue = UserRefClaimValue;
            
            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(true);
            AuthenticationService.Setup(a => a.TryGetCurrentUserClaimValue(DasClaimTypes.Id, out userRefClaimValue)).Returns(true);
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidUserRef()
        {
            UserRefClaimValue = "BBB";
            
            var userRefClaimValue = UserRefClaimValue;
            
            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(true);
            AuthenticationService.Setup(a => a.TryGetCurrentUserClaimValue(DasClaimTypes.Id, out userRefClaimValue)).Returns(true);
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetUnauthenticatedUser()
        {
            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(false);
            
            return this;
        }
    }
}