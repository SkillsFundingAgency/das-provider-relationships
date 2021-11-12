using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Helpers;

namespace SFA.DAS.ProviderRelationships.UnitTests.Services
{
    [TestFixture]
    public class WhenGettingProviderUrls
    {
        private string ukprn;
        private string baseUrl;
        private ProviderRelationshipsConfiguration providerRelationshipConfig;
        private ProviderUrls providerUrls;

        [SetUp]
        public void Init()
        {
            //Arrange
            ukprn = "10024689";
            baseUrl = "http://somewhere/";
            providerRelationshipConfig = new ProviderRelationshipsConfiguration();
            providerRelationshipConfig.ProviderPortalBaseUrl = baseUrl;
            providerUrls = new ProviderUrls(providerRelationshipConfig);
        }

        [Test]
        public void Then_The_Recruit_Home_Url_Is_Returned()
        {
            //Act
            var result = providerUrls.Recruit(ukprn);

            //Assert
            result.Should().Be($"{baseUrl}/{ukprn}");
        }

        [Test]
        public void Then_The_ProviderManageRecruitEmails_Is_Returned()
        {
            //Act
            var result = providerUrls.ProviderManageRecruitEmails(ukprn);

            //Assert
            result.Should().Be($"{baseUrl}/{ukprn}/notifications-manage");
        }
    }
}