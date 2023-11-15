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
            contextMock.SetupGet(c => c.Session).Returns(sessionMock.Object);
            contextMock.SetupGet(c => c.User.Identity.IsAuthenticated).Returns(true);
            contextMock.SetupGet(c => c.Request.Query).Returns(
                new QueryCollection(new Dictionary<string, StringValues>
                {
                    { "accountTasks", "true" }
                }));

            var requestDelegateMock = new Mock<RequestDelegate>();
            var middleware = new AccountTasksMiddleware(requestDelegateMock.Object);

            // Act
            await middleware.InvokeAsync(contextMock.Object);

            // Assert
            contextMock.VerifyGet(c => c.Session, Times.Once);
            contextMock.VerifyGet(c => c.User.Identity.IsAuthenticated, Times.Once);
            contextMock.VerifyGet(c => c.Request.Query, Times.Once);

            sessionMock.Verify(s => s.SetString("AccountTasks", "true"), Times.Once);
            contextMock.Object.Items["AccountTasks"].Should().Be(true);
        }

        [Test]
        public async Task InvokeAsync_AccountTasks_NotSet_NoChanges()
        {
            // Arrange
            var contextMock = new Mock<HttpContext>();
            var sessionMock = new Mock<ISession>();
            contextMock.SetupGet(c => c.Session).Returns(sessionMock.Object);
            contextMock.SetupGet(c => c.User.Identity.IsAuthenticated).Returns(true);
            contextMock.SetupGet(c => c.Request.Query).Returns(new QueryCollection());

            var requestDelegateMock = new Mock<RequestDelegate>();
            var middleware = new AccountTasksMiddleware(requestDelegateMock.Object);

            // Act
            await middleware.InvokeAsync(contextMock.Object);

            // Assert
            contextMock.VerifyGet(c => c.Session, Times.Never);
            contextMock.VerifyGet(c => c.User.Identity.IsAuthenticated, Times.Once);
            contextMock.VerifyGet(c => c.Request.Query, Times.Once);

            sessionMock.Verify(s => s.SetString("AccountTasks", "true"), Times.Never);
            contextMock.Object.Items.ContainsKey("AccountTasks").Should().BeFalse();
        }
    }