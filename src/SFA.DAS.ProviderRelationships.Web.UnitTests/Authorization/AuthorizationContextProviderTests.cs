using System;
using System.Web;
using System.Web.Routing;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization;
using SFA.DAS.EmployerUsers.WebClientComponents;
using SFA.DAS.HashingService;
using SFA.DAS.ProviderRelationships.Web.Authentication;
using SFA.DAS.ProviderRelationships.Web.Authorization;
using SFA.DAS.ProviderRelationships.Web.RouteValues;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Authorization
{
    [TestFixture]
    [Parallelizable]
    public class AuthorizationContextProviderTests : FluentTest<AuthorizationContextProviderTestsFixture>
    {
        [Test]
        public void GetAuthorizationContext_WhenAccountIdExistsAndIsValidAndUserIsAuthenticatedAndUserRefIsValidAndUserEmailIsValid_ThenShouldReturnAuthroizationContextWithAccountIdAndUserRefValues()
        {
            Run(f => f.SetValidAccountId().SetValidUserRef().SetValidUserEmail(), f => f.GetAuthorizationContext(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Get<long?>("AccountId").Should().Be(f.AccountId);
                r.Get<Guid?>("UserRef").Should().Be(f.UserRef);
            });
        }
        
        [Test]
        public void GetAuthorizationContext_WhenAccountIdDoesNotExistAndUserIsNotAuthenticated_ThenShouldReturnAuthroizationContextWithoutAccountIdAndUserRefValues()
        {
            Run(f => f.SetUnauthenticatedUser(), f => f.GetAuthorizationContext(), (f, r) =>
            {
                r.Should().NotBeNull();
                r.Get<long?>("AccountId").Should().BeNull();
                r.Get<Guid?>("UserRef").Should().BeNull();
            });
        }
        
        [Test]
        public void GetAuthorizationContext_WhenAccountIdExistsAndIsInvalid_ThenShouldThrowUnauthorizedAccessException()
        {
            Run(f => f.SetInvalidAccountId(), f => f.GetAuthorizationContext(), (f, r) => r.Should().Throw<UnauthorizedAccessException>());
        }
        
        [Test]
        public void GetAuthorizationContext_WhenUserIsAuthenticatedAndUserRefIsInvalid_ThenShouldThrowUnauthorizedAccessException()
        {
            Run(f => f.SetValidAccountId().SetInvalidUserRef(), f => f.GetAuthorizationContext(), (f, r) => r.Should().Throw<UnauthorizedAccessException>());
        }
        
        [Test]
        public void GetAuthorizationContext_WhenUserIsAuthenticatedAndUserEmailIsInvalid_ThenShouldThrowUnauthorizedAccessException()
        {
            Run(f => f.SetValidAccountId().SetValidUserRef().SetInvalidUserEmail(), f => f.GetAuthorizationContext(), (f, r) => r.Should().Throw<UnauthorizedAccessException>());
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
        public string UserEmail { get; set; }

        public AuthorizationContextProviderTestsFixture()
        {
            HttpContext = new Mock<HttpContextBase>();
            HashingService = new Mock<IHashingService>();
            AuthenticationService = new Mock<IAuthenticationService>();
            
            HttpContext.Setup(c => c.Request.RequestContext.RouteData).Returns(new RouteData());
            
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
            
            routeData.Values[RouteValueKeys.AccountHashedId] = AccountHashedId;
            
            HttpContext.Setup(c => c.Request.RequestContext.RouteData).Returns(routeData);
            HashingService.Setup(h => h.DecodeValue(AccountHashedId)).Returns(AccountId);
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidAccountId()
        {
            AccountHashedId = "AAA";

            var routeData = new RouteData();
            
            routeData.Values[RouteValueKeys.AccountHashedId] = AccountHashedId;
            
            HttpContext.Setup(c => c.Request.RequestContext.RouteData).Returns(routeData);
            HashingService.Setup(h => h.DecodeValue(AccountHashedId)).Throws<Exception>();
            
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

        public AuthorizationContextProviderTestsFixture SetValidUserEmail()
        {
            UserEmail = "foo@bar.com";
            
            var userEmailClaimValue = UserEmail;
            
            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(true);
            AuthenticationService.Setup(a => a.TryGetCurrentUserClaimValue(DasClaimTypes.Email, out userEmailClaimValue)).Returns(true);
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetInvalidUserEmail()
        {
            UserEmail = null;
            
            var userEmailClaimValue = UserEmail;
            
            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(true);
            AuthenticationService.Setup(a => a.TryGetCurrentUserClaimValue(DasClaimTypes.Email, out userEmailClaimValue)).Returns(false);
            
            return this;
        }

        public AuthorizationContextProviderTestsFixture SetUnauthenticatedUser()
        {
            AuthenticationService.Setup(a => a.IsUserAuthenticated()).Returns(false);
            
            return this;
        }
    }
}