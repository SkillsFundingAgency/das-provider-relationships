﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Client;
using SFA.DAS.ProviderRelationships.Application.Commands.RunHealthCheck;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Services;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class RunHealthCheckCommandHandlerTests : FluentTest<RunHealthCheckCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingARunHealthCheckCommand_ThenShouldAddAHealthCheck()
        {
            return TestAsync(f => f.Handle(), f => f.Db.HealthChecks.SingleOrDefault().Should().NotBeNull()
                .And.Match<HealthCheck>(h => h.User == f.User));
        }
    }

    public class RunHealthCheckCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public User User { get; set; }
        public RunHealthCheckCommand RunHealthCheckCommand { get; set; }
        public IRequestHandler<RunHealthCheckCommand> Handler { get; set; }
        public UnitOfWorkContext UnitOfWorkContext { get; set; }
        public Mock<IRoatpService> ProviderApiClient { get; set; }
        public Mock<IProviderRelationshipsApiClient> ProviderRelationshipsApiClient { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public RunHealthCheckCommandHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(
                new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            User = EntityActivator.CreateInstance<User>()
                .Set(u => u.Ref, Guid.NewGuid())
                .Set(u => u.Email, Guid.NewGuid().ToString())
                .Set(u => u.FirstName, Guid.NewGuid().ToString())
                .Set(u => u.LastName, Guid.NewGuid().ToString());
            RunHealthCheckCommand = new RunHealthCheckCommand(User.Ref);
            UnitOfWorkContext = new UnitOfWorkContext();
            ProviderApiClient = new Mock<IRoatpService>();
            ProviderRelationshipsApiClient = new Mock<IProviderRelationshipsApiClient>();
            CancellationToken = new CancellationToken();

            Db.Users.Add(User);
            Db.SaveChanges();
            
            ProviderApiClient.Setup(c => c.Ping()).ReturnsAsync(true);
            ProviderRelationshipsApiClient.Setup(c => c.Ping(CancellationToken)).Returns(Task.CompletedTask);

            Handler = new RunHealthCheckCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db), ProviderApiClient.Object, ProviderRelationshipsApiClient.Object);
        }

        public async Task Handle()
        {
            await Handler.Handle(RunHealthCheckCommand, CancellationToken);
            await Db.SaveChangesAsync();
        }
    }
}