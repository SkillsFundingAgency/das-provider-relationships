using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Helpers;

namespace SFA.DAS.ProviderRelationships.UnitTests.Services
{
    [TestFixture]
    public class WhenGettingProviderUrls
    {
        private string _ukprn;
        private string _baseUrl;
        private ProviderRelationshipsConfiguration _providerRelationshipConfig;
        private ProviderUrls _providerUrls;

        [SetUp]
        public void Init()
        {
            //Arrange
            _ukprn = "10024689";
            _baseUrl = "http://somewhere/";
            _providerRelationshipConfig = new ProviderRelationshipsConfiguration();
            _providerRelationshipConfig.ProviderPortalBaseUrl = _baseUrl;
            _providerUrls = new ProviderUrls(_providerRelationshipConfig);
        }

        [Test]
        public void Then_The_Recruit_Home_Url_Is_Returned()
        {
            //Act
            var result = _providerUrls.Recruit(_ukprn);

            //Assert
            result.Should().Be($"{_baseUrl}/{_ukprn}");
        }

        [Test]
        public void Then_The_ProviderManageRecruitEmails_Is_Returned()
        {
            //Act
            var result = _providerUrls.ProviderManageRecruitEmails(_ukprn);

            //Assert
            result.Should().Be($"{_baseUrl}/{_ukprn}/notifications-manage");
        }
    }
}