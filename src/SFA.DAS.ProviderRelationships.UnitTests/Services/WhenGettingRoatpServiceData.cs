using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Services;

namespace SFA.DAS.ProviderRelationships.UnitTests.Services
{
    public class WhenGettingDataFromRoatpService
    {
        [Test]
        public async Task Then_The_Endpoint_Is_Called_And_True_Returned_If_Ok_Response()
        {
            //Arrange
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject("test")),
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = SetupMessageHandlerMock(response, new Uri("https://test.local/api/ping"), HttpMethod.Get);
            
            var mockClient = new Mock<IRoatpApiHttpClientFactory>();
            mockClient.Setup(x => x.CreateRestHttpClient()).Returns(new RestHttpClient(new HttpClient(httpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test.local/api/")
            }));
            var service = new RoatpService(mockClient.Object);
            
            //Act
            var actual = await service.Ping();

            //Assert
            Assert.IsTrue(actual);

        }

        [Test]
        public async Task Then_If_Throws_Exception_Returns_False()
        {
            //Arrange
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject("test")),
                StatusCode = HttpStatusCode.BadRequest,
                ReasonPhrase = "test",
                RequestMessage = new HttpRequestMessage()
            };
            var httpMessageHandler = SetupMessageHandlerMock(response, new Uri("https://test.local/api/ping"), HttpMethod.Get);
            
            var mockClient = new Mock<IRoatpApiHttpClientFactory>();
            mockClient.Setup(x => x.CreateRestHttpClient()).Returns(new RestHttpClient(new HttpClient(httpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test.local/api/")
            }));
            var service = new RoatpService(mockClient.Object);
            
            //Act
            var actual = await service.Ping();

            //Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public async Task Then_The_Endpoint_Is_Called_And_Roatp_Data_Returned()
        {
            //Arrange
            var responseModel = new List<ProviderRegistration>
            {
                new ProviderRegistration
                {
                    Ukprn = 1,
                    ProviderName = "Test"
                },
                new ProviderRegistration
                {
                    Ukprn = 2,
                    ProviderName = "Test 2"
                },
            };
            var content = new StringContent(JsonConvert.SerializeObject(responseModel));
            content.Headers.Remove("Content-Type");
            content.Headers.Add("Content-Type", "application/json");
            var response = new HttpResponseMessage
            {
                Content = content,
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = SetupMessageHandlerMock(response, new Uri("https://test.local/api/v1/fat-data-export"), HttpMethod.Get);
            var mockClient = new Mock<IRoatpApiHttpClientFactory>();
            mockClient.Setup(x => x.CreateRestHttpClient()).Returns(new RestHttpClient(new HttpClient(httpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test.local/api/"),
            }));
            var service = new RoatpService(mockClient.Object);
            
            //Act
            var actual = await service.GetProviders();

            actual.Should().BeEquivalentTo(responseModel);
        }
        
        private static Mock<HttpMessageHandler> SetupMessageHandlerMock(HttpResponseMessage response, Uri uri, HttpMethod httpMethod)
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(httpMethod)
                        && c.RequestUri.Equals(uri)
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => response);
            return httpMessageHandler;
        }
    }
}