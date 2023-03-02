using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Services.OuterApi;

namespace SFA.DAS.ProviderRelationships.UnitTests.Services.OuterApi.Requests
{
    public class WhenConstructingGetEmployerAccountRequest
    {
        [Test, AutoData]
        public void Then_It_Is_Correctly_Constructed(string userId, string email)
        {
            //Arrange
            email = $"{email}@email.com";
            var actual = new GetEmployerAccountRequest(userId, email);

            //Assert
            actual.GetUrl.Should().Be($"accountusers/{userId}/accounts?email={WebUtility.UrlEncode(email)}");
        }
    }
}
