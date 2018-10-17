using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Types.Providers;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.Providers.Api.Client;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application
{
    [TestFixture]
    [Parallelizable]
    public class RunHealthCheckCommandHandlerTests : FluentTest<RunHealthCheckCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingARunHealthCheckCommand_ThenShouldAddAHealthCheck()
        {
            return RunAsync(f => f.Handle(), f => f.Db.HealthChecks.SingleOrDefault().Should().NotBeNull());
        }
    }

    public class RunHealthCheckCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public RunHealthCheckCommand RunHealthCheckCommand { get; set; }
        public IRequestHandler<RunHealthCheckCommand, Unit> Handler { get; set; }
        public UnitOfWorkContext UnitOfWorkContext { get; set; }
        public Mock<IProviderApiClient> ProviderApiClient { get; set; }

        public RunHealthCheckCommandHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            RunHealthCheckCommand = new RunHealthCheckCommand();
            UnitOfWorkContext = new UnitOfWorkContext();
            ProviderApiClient = new Mock<IProviderApiClient>();
            
            ProviderApiClient.Setup(c => c.SearchAsync("", 1)).ReturnsAsync(new ProviderSearchResponseItem());

            Handler = new RunHealthCheckCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ProviderApiClient.Object);
        }

        public async Task Handle()
        {
            await Handler.Handle(RunHealthCheckCommand, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}