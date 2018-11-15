using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
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
            Run(f => f.SetBaseUrlsWithTrailingSlash(), f => act(f.EmployerUrls), (f, r) => r.Should().Be($"http://example.com:12345{(expected!=""?"/":"")}{expected}"));
        }

        [TestCaseSource(nameof(_noAccountHashedIdTestCases))]
        public void WhenBaseUrlsHaveNoTrailingSlashAndGettingUrlWithoutAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string> act, string expected)
        {
            Run(f => f.SetBaseUrlsWithoutTrailingSlash(), f => act(f.EmployerUrls), (f, r) => r.Should().Be($"http://example.com:12345{(expected!=""?"/":"")}{expected}"));
        }

        [TestCaseSource(nameof(_accountHashedIdTestCases))]
        public void WhenBaseUrlsHaveTrailingSlashAndGettingUrlWithASuppliedAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string, string> act, string expected)
        {
            Run(f => f.SetBaseUrlsWithTrailingSlash(), f => act(f.EmployerUrls, "123456"), (f, r) => r.Should().Be($"http://example.com:12345/accounts/123456/{expected}"));
        }

        [TestCaseSource(nameof(_accountHashedIdTestCases))]
        public void WhenBaseUrlsHaveNoTrailingSlashAndGettingUrlWithASuppliedAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string, string> act, string expected)
        {
            Run(f => f.SetBaseUrlsWithoutTrailingSlash(), f => act(f.EmployerUrls, "123456"), (f, r) => r.Should().Be($"http://example.com:12345/accounts/123456/{expected}"));
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
            new object[] { (Func<EmployerUrls, string>)(eu => eu.Homepage()), "" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.YourAccounts()), "service/accounts" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.NotificationSettings()), "settings/notifications" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.SignIn()), "service/signin" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.SignOut()), "service/signout" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.Help()), "service/help" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.Privacy()), "service/privacy" }
        };

        private static object[] _accountHashedIdTestCases =
        {
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.RenameAccount(ahid)), "rename" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.YourTeam(ahid)), "teams/view" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.YourOrganisationsAndAgreements(ahid)), "agreements" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.PayeSchemes(ahid)), "schemes" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.Apprentices(ahid)), "apprentices/home" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.AccountHomepage(ahid)), "teams" },
            new object[] { (Func<EmployerUrls, string, string>)((eu, ahid) => eu.FinanceHomepage(ahid)), "finance" }
        };
    }
    
    public class EmployerUrlsTestsFixture
    {
        public EmployerUrls EmployerUrls { get; set; }
        public ProviderRelationshipsConfiguration Configuration { get; set; }
        
        public EmployerUrlsTestsFixture()
        {
            Configuration = new ProviderRelationshipsConfiguration();
            EmployerUrls = new EmployerUrls(Configuration);
        }

        public void SetBaseUrlsWithTrailingSlash()
        {
            const string baseUrl = "http://example.com:12345/";

            Configuration.EmployerAccountsBaseUrl = baseUrl;
            Configuration.EmployerCommitmentsBaseUrl = baseUrl;
            Configuration.EmployerFinanceBaseUrl = baseUrl;
            Configuration.EmployerPortalBaseUrl = baseUrl;
            Configuration.EmployerProjectionsBaseUrl = baseUrl;
            Configuration.EmployerRecruitBaseUrl = baseUrl;
        }
        
        public void SetBaseUrlsWithoutTrailingSlash()
        {
            const string baseUrl = "http://example.com:12345";

            Configuration.EmployerAccountsBaseUrl = baseUrl;
            Configuration.EmployerCommitmentsBaseUrl = baseUrl;
            Configuration.EmployerFinanceBaseUrl = baseUrl;
            Configuration.EmployerPortalBaseUrl = baseUrl;
            Configuration.EmployerProjectionsBaseUrl = baseUrl;
            Configuration.EmployerRecruitBaseUrl = baseUrl;
        }
    }
}