using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Helpers;

namespace SFA.DAS.ProviderRelationships.UnitTests.Services
{
    public class WhenGettingProviderUrls
    {
        [Test]
        public void Then_The_Recruit_Home_Url_Is_Returned()
        {
            //Arrange
            var ukprn = "10024689";
            var baseUrl = "http://somewhere/";
            var providerRelationshipConfig = new ProviderRelationshipsConfiguration();
            providerRelationshipConfig.ProviderPortalBaseUrl = baseUrl;
            var providerUrls = new ProviderUrls(providerRelationshipConfig);

            //Act
            var result = providerUrls.Recruit(ukprn);

            //Assert
            result.Should().Be($"{baseUrl}/{ukprn}");
        }

        [Test]
        public void Then_The_ProviderManageRecruitEmails_Is_Returned()
        {
            //Arrange
            var ukprn = "10024689";
            var baseUrl = "http://somewhere/";
            var providerRelationshipConfig = new ProviderRelationshipsConfiguration();
            providerRelationshipConfig.ProviderPortalBaseUrl = baseUrl;
            var providerUrls = new ProviderUrls(providerRelationshipConfig);

            //Act
            var result = providerUrls.ProviderManageRecruitEmails(ukprn);

            //Assert
            result.Should().Be($"{baseUrl}/{ukprn}/notifications-manage");
        }
    }
}