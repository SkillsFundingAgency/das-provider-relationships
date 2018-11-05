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
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Providers.Api.Client;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class RunHealthCheckCommandHandlerTests : FluentTest<RunHealthCheckCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingARunHealthCheckCommand_ThenShouldAddAHealthCheck()
        {
            return RunAsync(f => f.Handle(), f => f.Db.HealthChecks.SingleOrDefault().Should().NotBeNull()
                .And.Match<HealthCheck>(h => h.User == f.User));
        }
    }

    public class RunHealthCheckCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public User User { get; set; }
        public RunHealthCheckCommand RunHealthCheckCommand { get; set; }
        public IRequestHandler<RunHealthCheckCommand, Unit> Handler { get; set; }
        public UnitOfWorkContext UnitOfWorkContext { get; set; }
        public Mock<IProviderApiClient> ProviderApiClient { get; set; }

        public RunHealthCheckCommandHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            User = new UserBuilder().WithRef(Guid.NewGuid());
            RunHealthCheckCommand = new RunHealthCheckCommand(User.Ref);
            UnitOfWorkContext = new UnitOfWorkContext();
            ProviderApiClient = new Mock<IProviderApiClient>();

            Db.Users.Add(User);
            Db.SaveChanges();
            
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