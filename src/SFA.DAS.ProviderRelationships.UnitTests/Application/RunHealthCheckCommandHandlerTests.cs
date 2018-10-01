using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Types.Providers;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Providers.Api.Client;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application
{
    [TestFixture]
    public class RunHealthCheckCommandHandlerTests : FluentTest<RunHealthCheckCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingARunHealthCheckCommand_ThenShouldAddAHealthCheck()
        {
            return RunAsync(f => f.Handle(), f => f.Db.Verify(d => d.HealthChecks.Add(It.IsAny<HealthCheck>())));
        }
    }

    public class RunHealthCheckCommandHandlerTestsFixture
    {
        public Mock<ProviderRelationshipsDbContext> Db { get; set; }
        public RunHealthCheckCommand RunHealthCheckCommand { get; set; }
        public IRequestHandler<RunHealthCheckCommand, Unit> Handler { get; set; }
        public UnitOfWorkContext UnitOfWorkContext { get; set; }
        public Mock<IProviderApiClient> ProviderApiClient { get; set; }

        public RunHealthCheckCommandHandlerTestsFixture()
        {
            Db = new Mock<ProviderRelationshipsDbContext>();
            RunHealthCheckCommand = new RunHealthCheckCommand { UserRef = Guid.NewGuid() };
            UnitOfWorkContext = new UnitOfWorkContext();
            ProviderApiClient = new Mock<IProviderApiClient>();

            Db.Setup(d => d.HealthChecks.Add(It.IsAny<HealthCheck>()));
            ProviderApiClient.Setup(c => c.SearchAsync("", 1)).ReturnsAsync(new ProviderSearchResponseItem());

            Handler = new RunHealthCheckCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db.Object), ProviderApiClient.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(RunHealthCheckCommand, CancellationToken.None);
        }
    }
}