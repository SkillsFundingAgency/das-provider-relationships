using System;
using System.Net;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Urls;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Urls
{
    [TestFixture]
    public class EmployerUrlsTests : FluentTest<EmployerUrlsTestsFixture>
    {
        [TestCaseSource(nameof(_noAccountHashedIdTestCases))]
        public void WhenBaseUrlsHaveTrailingSlashAndGettingUrlWithoutAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string> act, string expected)
        {
            Run(f => f.SetBaseUrlsWithTrailingSlash(), f => act(f.EmployerUrls), (f, r) => r.Should().Be($"http://example.com:12345{(expected != "" ? "/" : "")}{expected}"));
        }

        [TestCaseSource(nameof(_noAccountHashedIdTestCases))]
        public void WhenBaseUrlsHaveNoTrailingSlashAndGettingUrlWithoutAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string> act, string expected)
        {
            Run(f => f.SetBaseUrlsWithoutTrailingSlash(), f => act(f.EmployerUrls), (f, r) => r.Should().Be($"http://example.com:12345{(expected != "" ? "/" : "")}{expected}"));
        }

        [TestCaseSource(nameof(_accountHashedIdTestCases))]
        public void WhenBaseUrlsHaveTrailingSlashAndGettingUrlWithASuppliedAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string, string> act, string expected)
        {
            Run(f => f.SetBaseUrlsWithTrailingSlash(), f => act(f.EmployerUrls, "123456"), (f, r) => r.Should().Be($"http://example.com:12345/accounts/123456{(expected != "" ? "/" : "")}{expected}"));
        }

        [TestCaseSource(nameof(_accountHashedIdTestCases))]
        public void WhenBaseUrlsHaveNoTrailingSlashAndGettingUrlWithASuppliedAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string, string> act, string expected)
        {
            Run(f => f.SetBaseUrlsWithoutTrailingSlash(), f => act(f.EmployerUrls, "123456"), (f, r) => r.Should().Be($"http://example.com:12345/accounts/123456{(expected != "" ? "/" : "")}{expected}"));
        }

        [TestCaseSource(nameof(_accountHashedIdTestCases))]
        public void WhenBaseUrlsHaveTrailingSlashAndGettingUrlWithANullAccountHashedId_ThenAnExceptionShouldBeThrown(Func<EmployerUrls, string, string> act, string expected)
        {
            Run(f => f.SetBaseUrlsWithTrailingSlash(), f => act(f.EmployerUrls, null),
                (f, r) => r.Should().Throw<ArgumentException>());
        }

        [TestCaseSource(nameof(_accountHashedIdTestCases))]
        public void WhenBaseUrlsHaveNoTrailingSlashAndGettingUrlWithANullAccountHashedId_ThenAnExceptionShouldBeThrown(Func<EmployerUrls, string, string> act, string expected)
        {
            Run(f => f.SetBaseUrlsWithoutTrailingSlash(), f => act(f.EmployerUrls, null),
                (f, r) => r.Should().Throw<ArgumentException>());
        }
        
        [TestCaseSource(nameof(_noAccountHashedIdTestCases))]
        public void WhenBaseUrlsAreMissingAndGettingUrlsThatDontIncludeAccountHashedId_ThenAnExceptionShouldBeThrown(Func<EmployerUrls, string> act, string expected)
        {
            Run(f => act(f.EmployerUrls), (f, r) => r.Should().Throw<ArgumentException>());
        }

        [TestCaseSource(nameof(_accountHashedIdTestCases))]
        public void WhenBaseUrlsAreMissingAndGettingUrlsThatIncludeAccountHashedId_ThenAnExceptionShouldBeThrown(Func<EmployerUrls, string, string> act, string expected)
        {
            Run(f => act(f.EmployerUrls, "123456"), (f, r) => r.Should().Throw<ArgumentException>());
        }

        private static object[] _noAccountHashedIdTestCases =
        {
            new object[] { (Func<EmployerUrls, string>)(eu => eu.ChangeEmail()), $"account/changeemail?clientId=abc123&returnurl={WebUtility.UrlEncode("http://example.com:12345/service/email/change")}" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.ChangePassword()), $"account/changepassword?clientId=abc123&returnurl={WebUtility.UrlEncode("http://example.com:12345/service/password/change")}" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.Help()), "service/help" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.Homepage()), "" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.NotificationSettings()), "settings/notifications" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.Privacy()), "service/privacy" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.SignIn()), "service/signin" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.SignOut()), "service/signout" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.YourAccounts()), "service/accounts" }
        };

        private static object[] _accountHashedIdTestCases =
        {
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.Account(ahid)), "teams" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.Apprentices(ahid)), "apprentices/home" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.Finance(ahid)), "finance" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.PayeSchemes(ahid)), "schemes" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.Recruit(ahid)), "" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.RenameAccount(ahid)), "rename" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.YourOrganisationsAndAgreements(ahid)), "agreements" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.YourTeam(ahid)), "teams/view" }
        };
    }
    
    public class EmployerUrlsTestsFixture
    {
        public EmployerUrls EmployerUrls { get; set; }
        public Mock<IEmployerUrlsConfiguration> EmployerUrlsConfiguration { get; set; }
        public Mock<IIdentityServerConfiguration> IdentityServerConfiguration { get; set; }
        
        public EmployerUrlsTestsFixture()
        {
            EmployerUrlsConfiguration = new Mock<IEmployerUrlsConfiguration>();
            IdentityServerConfiguration = new Mock<IIdentityServerConfiguration>();

            IdentityServerConfiguration.Setup(c => c.ClientId).Returns("abc123");
            
            EmployerUrls = new EmployerUrls(EmployerUrlsConfiguration.Object, IdentityServerConfiguration.Object);
        }

        public void SetBaseUrlsWithTrailingSlash()
        {
            const string baseUrl = "http://example.com:12345/";

            EmployerUrlsConfiguration.Setup(c => c.EmployerAccountsBaseUrl).Returns(baseUrl);
            EmployerUrlsConfiguration.Setup(c => c.EmployerCommitmentsBaseUrl).Returns(baseUrl);
            EmployerUrlsConfiguration.Setup(c => c.EmployerFinanceBaseUrl).Returns(baseUrl);
            EmployerUrlsConfiguration.Setup(c => c.EmployerPortalBaseUrl).Returns(baseUrl);
            EmployerUrlsConfiguration.Setup(c => c.EmployerProjectionsBaseUrl).Returns(baseUrl);
            EmployerUrlsConfiguration.Setup(c => c.EmployerRecruitBaseUrl).Returns(baseUrl);
            EmployerUrlsConfiguration.Setup(c => c.EmployerUsersBaseUrl).Returns(baseUrl);
        }
        
        public void SetBaseUrlsWithoutTrailingSlash()
        {
            const string baseUrl = "http://example.com:12345";

            EmployerUrlsConfiguration.Setup(c => c.EmployerAccountsBaseUrl).Returns(baseUrl);
            EmployerUrlsConfiguration.Setup(c => c.EmployerCommitmentsBaseUrl).Returns(baseUrl);
            EmployerUrlsConfiguration.Setup(c => c.EmployerFinanceBaseUrl).Returns(baseUrl);
            EmployerUrlsConfiguration.Setup(c => c.EmployerPortalBaseUrl).Returns(baseUrl);
            EmployerUrlsConfiguration.Setup(c => c.EmployerProjectionsBaseUrl).Returns(baseUrl);
            EmployerUrlsConfiguration.Setup(c => c.EmployerRecruitBaseUrl).Returns(baseUrl);
            EmployerUrlsConfiguration.Setup(c => c.EmployerUsersBaseUrl).Returns(baseUrl);
        }
    }
}