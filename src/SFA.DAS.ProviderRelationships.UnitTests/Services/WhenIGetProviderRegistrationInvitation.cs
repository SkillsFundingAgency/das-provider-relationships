using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services;

namespace SFA.DAS.ProviderRelationships.UnitTests.Services;

class WhenIGetInvitation
{
    private IRegistrationApiClient _sut;
    private RegistrationApiConfiguration _registrationApiConfiguration;
    private string _correlationId;
    private string _testData;
    private string _apiBaseUrl;
    private string _identifierUri;
    Mock<HttpMessageHandler> _mockHttpMessageHandler;

    [SetUp]
    public void Arrange()
    {
        _apiBaseUrl = $"http://{Guid.NewGuid()}/";
        _identifierUri = Guid.NewGuid().ToString();
        _correlationId = Guid.NewGuid().ToString();
        _testData = "Employer details";

        _registrationApiConfiguration = new RegistrationApiConfiguration {
            ApiBaseUrl = _apiBaseUrl,
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

        var configuration = GenerateConfiguration();

        _sut = new RegistrationApiClient(httpClient, _registrationApiConfiguration, configuration);
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
                                                   && r.RequestUri.AbsoluteUri == $"{_registrationApiConfiguration.ApiBaseUrl}api/invitations/{_correlationId}"),
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
                                                   && r.RequestUri.AbsoluteUri == $"{_registrationApiConfiguration.ApiBaseUrl}api/unsubscribe/{_correlationId}"),
                ItExpr.IsAny<CancellationToken>());
    }


    private IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource { InitialData = new List<KeyValuePair<string, string>> { new("EnvironmentName", "LOCAL") } };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}