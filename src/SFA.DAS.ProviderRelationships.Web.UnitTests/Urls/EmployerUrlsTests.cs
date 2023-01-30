using System;
using System.Net;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Urls
{
    [TestFixture]
    [Parallelizable]
    public class EmployerUrlsTests : FluentTest<EmployerUrlsTestsFixture>
    {
        [TestCaseSource(nameof(_noAccountHashedIdTestCases))]
        public void WhenBaseUrlsHaveTrailingSlashAndGettingUrlWithoutAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string> act, string expected)
        {
            Test(f => f.SetBaseUrlsWithTrailingSlash(), f => act(f.EmployerUrls), (f, r) => r.Should().Be($"http://example.com:12345{(expected != "" ? "/" : "")}{expected}"));
        }

        [TestCaseSource(nameof(_noAccountHashedIdTestCases))]
        public void WhenBaseUrlsHaveNoTrailingSlashAndGettingUrlWithoutAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string> act, string expected)
        {
            Test(f => f.SetBaseUrlsWithoutTrailingSlash(), f => act(f.EmployerUrls), (f, r) => r.Should().Be($"http://example.com:12345{(expected != "" ? "/" : "")}{expected}"));
        }

        [TestCaseSource(nameof(_accountHashedIdTestCases))]
        public void WhenBaseUrlsHaveTrailingSlashAndGettingUrlWithASuppliedAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string, string> act, string expected)
        {
            Test(f => f.SetBaseUrlsWithTrailingSlash(), f => act(f.EmployerUrls, "123456"), (f, r) => r.Should().Be($"http://example.com:12345/accounts/123456{(expected != "" ? "/" : "")}{expected}"));
        }

        [TestCaseSource(nameof(_accountHashedIdTestCases))]
        public void WhenBaseUrlsHaveNoTrailingSlashAndGettingUrlWithASuppliedAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string, string> act, string expected)
        {
            Test(f => f.SetBaseUrlsWithoutTrailingSlash(), f => act(f.EmployerUrls, "123456"), (f, r) => r.Should().Be($"http://example.com:12345/accounts/123456{(expected != "" ? "/" : "")}{expected}"));
        }

        [TestCaseSource(nameof(_accountHashedIdTestCases))]
        public void WhenBaseUrlsHaveTrailingSlashAndGettingUrlWithANullAccountHashedId_ThenAnExceptionShouldBeThrown(Func<EmployerUrls, string, string> act, string expected)
        {
            TestException(f => f.SetBaseUrlsWithTrailingSlash(), f => act(f.EmployerUrls, null),
                (f, r) => r.Should().Throw<ArgumentException>());
        }

        [TestCaseSource(nameof(_accountHashedIdTestCases))]
        public void WhenBaseUrlsHaveNoTrailingSlashAndGettingUrlWithANullAccountHashedId_ThenAnExceptionShouldBeThrown(Func<EmployerUrls, string, string> act, string expected)
        {
            TestException(f => f.SetBaseUrlsWithoutTrailingSlash(), f => act(f.EmployerUrls, null),
                (f, r) => r.Should().Throw<ArgumentException>());
        }
        
        [TestCaseSource(nameof(_noAccountHashedIdTestCases))]
        public void WhenBaseUrlsAreMissingAndGettingUrlsThatDontIncludeAccountHashedId_ThenAnExceptionShouldBeThrown(Func<EmployerUrls, string> act, string expected)
        {
            TestException(f => act(f.EmployerUrls), (f, r) => r.Should().Throw<ArgumentException>());
        }

        [TestCaseSource(nameof(_accountHashedIdTestCases))]
        public void WhenBaseUrlsAreMissingAndGettingUrlsThatIncludeAccountHashedId_ThenAnExceptionShouldBeThrown(Func<EmployerUrls, string, string> act, string expected)
        {
            TestException(f => act(f.EmployerUrls, "123456"), (f, r) => r.Should().Throw<ArgumentException>());
        }

        private static object[] _noAccountHashedIdTestCases =
        {
            new object[] { (Func<EmployerUrls, string>)(eu => eu.Homepage()), "" },
        };

        private static object[] _accountHashedIdTestCases =
        {
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.Account(ahid)), "teams" },
        };
    }
    
    public class EmployerUrlsTestsFixture
    {
        public EmployerUrls EmployerUrls { get; set; }
        public Mock<IEmployerUrlsConfiguration> EmployerUrlsConfiguration { get; set; }
        
        public EmployerUrlsTestsFixture()
        {
            EmployerUrlsConfiguration = new Mock<IEmployerUrlsConfiguration>();
            
            EmployerUrls = new EmployerUrls(EmployerUrlsConfiguration.Object);
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