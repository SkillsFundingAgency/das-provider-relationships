using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services.OuterApi;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRelationships.UnitTests.Services.OuterApi;

public class WhenCallingGet
{
    [Test]
    [MoqAutoData]
    public async Task Then_The_Endpoint_Is_Called_With_Authentication_Header_And_Data_Returned(
        string key,
        string baseUrl,
        GetTestRequest request,
        List<string> responseContent)
    {
        //Arrange
        var config = new OuterApiConfiguration {BaseUrl = $"https://{baseUrl.ToLower()}/", Key = key};
        var response = new HttpResponseMessage
        {
            Content = new StringContent(JsonConvert.SerializeObject(responseContent)),
            StatusCode = HttpStatusCode.Accepted
        };
        var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, $"{config.BaseUrl}{request.GetUrl}", config.Key);
        var client = new HttpClient(httpMessageHandler.Object);
        var apiClient = new OuterApiClient(client, config);

        //Act
        var actual = await apiClient.Get<List<string>>(request);
            
        //Assert
        actual.Should().BeEquivalentTo(responseContent);
    }
    
    [Test]
    [MoqAutoData]
    public void And_Not_Successful_Then_An_Exception_Is_Thrown(
        string key,
        string baseUrl,
        GetTestRequest request)
    {
        //Arrange
        var config = new OuterApiConfiguration {BaseUrl = $"https://{baseUrl.ToLower()}/", Key = key};
        var response = new HttpResponseMessage
        {
            Content = new StringContent(""),
            StatusCode = HttpStatusCode.BadRequest
        };
            
        var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, $"{config.BaseUrl}{request.GetUrl}", config.Key);
        var client = new HttpClient(httpMessageHandler.Object);
        var apiClient = new OuterApiClient(client, config);
            
        //Act Assert
        Assert.ThrowsAsync<HttpRequestException>(() => apiClient.Get<List<string>>(request));
    }
    
    [Test]
    [MoqAutoData]
    public async Task And_Not_Found_Then_Default_Object_Is_Returned(
        string key,
        string baseUrl,
        GetTestRequest request)
    {
        //Arrange
        var config = new OuterApiConfiguration {BaseUrl = $"https://{baseUrl.ToLower()}/", Key = key};
        var response = new HttpResponseMessage
        {
            Content = new StringContent(""),
            StatusCode = HttpStatusCode.NotFound
        };

        var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, $"{config.BaseUrl}{request.GetUrl}", config.Key);
        var client = new HttpClient(httpMessageHandler.Object);
        var apiClient = new OuterApiClient(client, config);

        //Act
        var result = await apiClient.Get<List<string>>(request);

        // Assert
        result.Should().BeNull();
    }
}

public class GetTestRequest : IGetApiRequest
{
    public string GetUrl => "test-url/get";
}

public static class MessageHandler
{
    public static Mock<HttpMessageHandler> SetupMessageHandlerMock(HttpResponseMessage response, string url, string key)
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();
        httpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(c =>
                    c.Method.Equals(HttpMethod.Get)
                    && c.Headers.Contains("Ocp-Apim-Subscription-Key")
                    && c.Headers.GetValues("Ocp-Apim-Subscription-Key").First().Equals(key)
                    && c.Headers.Contains("X-Version")
                    && c.Headers.GetValues("X-Version").First().Equals("1")
                    && c.RequestUri.AbsoluteUri.Equals(url)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);
        return httpMessageHandler;
    }
}