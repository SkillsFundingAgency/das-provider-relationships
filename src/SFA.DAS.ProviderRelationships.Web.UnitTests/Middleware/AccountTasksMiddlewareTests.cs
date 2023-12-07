using Microsoft.Extensions.Primitives;
using SFA.DAS.ProviderRelationships.Web.Middleware;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.Middleware;

[TestFixture]
public class AccountTasksMiddlewareTests
{
        [Test]
        public async Task InvokeAsync_AccountTasks_QueryStringSet_SessionAndContextItemsUpdated()
        {
            // Arrange
            var contextMock = new Mock<HttpContext>();
            var sessionMock = new Mock<ISession>();
            contextMock.SetupGet(c => c.Items).Returns(new Dictionary<object, object>());
            contextMock.SetupGet(c => c.Session).Returns(sessionMock.Object);
            contextMock.SetupGet(c => c.User.Identity.IsAuthenticated).Returns(true);
            contextMock.SetupGet(c => c.Request.Query).Returns(
                new QueryCollection(new Dictionary<string, StringValues>
                {
                    { "AccountTasks", "true" }
                }));

            var requestDelegateMock = new Mock<RequestDelegate>();
            var middleware = new AccountTasksMiddleware(requestDelegateMock.Object);

            // Act
            await middleware.InvokeAsync(contextMock.Object);

            // Assert
            sessionMock.Verify(x => x.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny), Times.Once);
            contextMock.VerifyGet(c => c.User.Identity.IsAuthenticated, Times.Once);
            contextMock.VerifyGet(c => c.Request.Query, Times.Once);

            sessionMock.Verify(s => s.Set("AccountTasksKey", It.IsAny<byte[]>()), Times.Once);
            contextMock.Object.Items.Should().ContainKey("AccountTasksKey");
        }

        [Test]
        public async Task InvokeAsync_AccountTasks_NotSet_NoChanges()
        {
            // Arrange
            var contextMock = new Mock<HttpContext>();
            var sessionMock = new Mock<ISession>();
            contextMock.SetupGet(c => c.Items).Returns(new Dictionary<object, object>());
            contextMock.SetupGet(c => c.Session).Returns(sessionMock.Object);
            contextMock.SetupGet(c => c.User.Identity.IsAuthenticated).Returns(true);
            contextMock.SetupGet(c => c.Request.Query).Returns(new QueryCollection());

            var requestDelegateMock = new Mock<RequestDelegate>();
            var middleware = new AccountTasksMiddleware(requestDelegateMock.Object);

            // Act
            await middleware.InvokeAsync(contextMock.Object);

            // Assert
            sessionMock.Verify(x => x.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny), Times.Once);
            contextMock.VerifyGet(c => c.User.Identity.IsAuthenticated, Times.Once);
            contextMock.VerifyGet(c => c.Request.Query, Times.Once);

            sessionMock.Verify(s => s.Set("AccountTasks", It.IsAny<byte[]>()), Times.Never);
            contextMock.Object.Items.Should().NotContainKey("AccountTasksKey");
        }
    }