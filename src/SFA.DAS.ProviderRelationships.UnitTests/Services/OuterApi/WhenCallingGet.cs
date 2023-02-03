using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services.OuterApi;

namespace SFA.DAS.ProviderRelationships.UnitTests.Services.OuterApi;

public class WhenCallingGet
    {
        [Test]
        [AutoData]
        public async Task Then_The_Endpoint_Is_Called_With_Authentication_Header_And_Data_Returned(
            string key,
            string baseUrl,
            GetTestRequest request,
            List<string> responseContent)
        {
            //Arrange
            var config = new OuterApiConfiguration {BaseUrl = $"https://{baseUrl.ToLower()}/", Key = key};
            var response = new HttpResponseMessage {
                Content = new StringContent(JsonConvert.SerializeObject(responseContent)),
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(
                response, new Uri($"{config.BaseUrl}{request.GetUrl}"), config.Key, HttpMethod.Get);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new OuterApiClient(client, config);

            //Act
            var actual = await apiClient.Get<List<string>>(request);

            //Assert
            actual.Should().BeEquivalentTo(responseContent);
        }

        [Test]
        [AutoData]
        public void And_Not_Successful_Then_An_Exception_Is_Thrown(
            string key,
            string baseUrl,
            GetTestRequest request)
        {
            //Arrange
            var config = new OuterApiConfiguration {BaseUrl = $"https://{baseUrl.ToLower()}/", Key = key};
            var response = new HttpResponseMessage {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.BadRequest
            };

            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(
                response, new Uri($"{config.BaseUrl}{request.GetUrl}"), config.Key, HttpMethod.Get);
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new OuterApiClient(client, config);

            //Act Assert
            Assert.ThrowsAsync<HttpRequestException>(() => apiClient.Get<List<string>>(request));
        }

        [Test]
        [AutoData]
        public async Task And_Not_Found_Then_Default_Object_Is_Returned(
            string key,
            string baseUrl,
            GetTestRequest request)
        {
            //Arrange
            var config = new OuterApiConfiguration {BaseUrl = $"https://{baseUrl.ToLower()}/", Key = key};
            var response = new HttpResponseMessage {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.NotFound
            };

            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(
                response, new Uri($"{config.BaseUrl}{request.GetUrl}"), config.Key, HttpMethod.Get);
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