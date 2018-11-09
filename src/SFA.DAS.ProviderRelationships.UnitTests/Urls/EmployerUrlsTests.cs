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
        [TestCaseSource(nameof(NoAccountHashedIdTestCases))]
        public void WhenBaseUrlsHaveTrailingSlashAndGettingUrlWithoutAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string> act, string expected)
        {
            Run(f => f.SetBaseUrlsWithTrailingSlash(), f => act(f.EmployerUrls), (f, r) => r.Should().Be($"http://example.com:12345/{expected}"));
        }

        [TestCaseSource(nameof(NoAccountHashedIdTestCases))]
        public void WhenBaseUrlsHaveNoTrailingSlashAndGettingUrlWithoutAccountHashedId_ThenCorrectUrlShouldBeReturned(Func<EmployerUrls, string> act, string expected)
        {
            Run(f => f.SetBaseUrlsWithoutTrailingSlash(), f => act(f.EmployerUrls), (f, r) => r.Should().Be($"http://example.com:12345/{expected}"));
        }

        private static object[] NoAccountHashedIdTestCases =
        {
            new object[] { (Func<EmployerUrls, string>)(eu => eu.YourAccounts()), "service/accounts" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.NotificationSettings()), "settings/notifications" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.SignIn()), "service/signin" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.SignOut()), "service/signout" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.Help()), "service/help" },
            new object[] { (Func<EmployerUrls, string>)(eu => eu.Privacy()), "service/privacy" }
        };
    }
    
    public class EmployerUrlsTestsFixture
    {
        public EmployerUrls EmployerUrls { get; set; }
        public ProviderRelationshipsConfiguration Config { get; set; }
        
        public EmployerUrlsTestsFixture()
        {
            Config = new ProviderRelationshipsConfiguration();
            EmployerUrls = new EmployerUrls(Config);
        }

        public void SetBaseUrlsWithTrailingSlash()
        {
            const string baseUrl = "http://example.com:12345/";

            Config.EmployerAccountsBaseUrl = baseUrl;
            Config.EmployerCommitmentsBaseUrl = baseUrl;
            Config.EmployerFinanceBaseUrl = baseUrl;
            Config.EmployerPortalBaseUrl = baseUrl;
            Config.EmployerProjectionsBaseUrl = baseUrl;
            Config.EmployerRecruitBaseUrl = baseUrl;
        }
        
        public void SetBaseUrlsWithoutTrailingSlash()
        {
            const string baseUrl = "http://example.com:12345";

            Config.EmployerAccountsBaseUrl = baseUrl;
            Config.EmployerCommitmentsBaseUrl = baseUrl;
            Config.EmployerFinanceBaseUrl = baseUrl;
            Config.EmployerPortalBaseUrl = baseUrl;
            Config.EmployerProjectionsBaseUrl = baseUrl;
            Config.EmployerRecruitBaseUrl = baseUrl;
        }
    }
}