using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services;

namespace SFA.DAS.EmployerAccounts.UnitTests.Services.ProviderRegistration
{
    class WhenIGetInvitation
    {
        private IRegistrationApiClient _sut;
        private RegistrationApiConfiguration _configuration;
        private string _correlationId;
        private string _testData;
        private string _apiBaseUrl;
        private string _identifierUri;
        Mock<HttpMessageHandler> _mockHttpMessageHandler;
        Mock<ILogger<RegistrationApiClient>> _logger;

        [SetUp]
        public void Arrange()
        {
            ConfigurationManager.AppSettings["EnvironmentName"] = "LOCAL";
            _apiBaseUrl = $"http://{Guid.NewGuid().ToString()}/";
            _identifierUri = Guid.NewGuid().ToString();
            _correlationId = Guid.NewGuid().ToString();
            _testData = "Employer details";

            _logger = new Mock<ILogger<RegistrationApiClient>>();

            _configuration = new RegistrationApiConfiguration 
            {
                BaseUrl = _apiBaseUrl,
                IdentifierUri = _identifierUri
            };

            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            _mockHttpMessageHandler
                  .Protected()
                  .Setup<Task<HttpResponseMessage>>("SendAsync",
                      ItExpr.IsAny<HttpRequestMessage>(),
                      ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage {
                      Content = new StringContent(_testData),
                      StatusCode = HttpStatusCode.OK
                  }
                  ).Verifiable("");

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            var httpRequestMessage = new HttpRequestMessage();

            _sut = new RegistrationApiClient(httpClient, _configuration, httpRequestMessage);
        }

        [Test]
        public async Task Then_Verify_ProviderRegistrationApi_ToGetInvitationIsCalled()
        {
            //arrange
            CancellationToken cancellationToken = new CancellationToken();

            //act
            await _sut.GetInvitations(_correlationId, cancellationToken);

            //assert
            _mockHttpMessageHandler
                .Protected()
                .Verify("SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get
                                                       && r.RequestUri.AbsoluteUri == $"{_configuration.BaseUrl}api/invitations/{_correlationId}"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task WhenUnsubscribeProviderEmail_Then_Verify_ProviderRegistrationApiToUnsubscribeIsCalled()
        {
            //act
            await _sut.Unsubscribe(_correlationId);

            //assert
            _mockHttpMessageHandler
                .Protected()
                .Verify("SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get
                                                       && r.RequestUri.AbsoluteUri == $"{_configuration.BaseUrl}api/unsubscribe/{_correlationId}"),
                ItExpr.IsAny<CancellationToken>());
        }

    }
}