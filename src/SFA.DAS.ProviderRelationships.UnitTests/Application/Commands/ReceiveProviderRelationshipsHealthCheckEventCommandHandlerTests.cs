using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.ReceiveProviderRelationshipsHealthCheckEvent;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class ReceiveProviderRelationshipsHealthCheckEventCommandHandlerTests : FluentTest<ReceiveProviderRelationshipsHealthCheckEventCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingReceiveProviderRelationshipsHealthCheckEventCommand_ThenShouldUpdateHealthCheck()
        {
            return RunAsync(f => f.Handle(), f => f.HealthChecks[1].ReceivedProviderRelationshipsEvent.Should().NotBeNull());
        }
    }

    public class ReceiveProviderRelationshipsHealthCheckEventCommandHandlerTestsFixture
    {
        public List<HealthCheck> HealthChecks { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public ReceiveProviderRelationshipsHealthCheckEventCommand Command { get; set; }
        public IRequestHandler<ReceiveProviderRelationshipsHealthCheckEventCommand, Unit> Handler { get; set; }

        public ReceiveProviderRelationshipsHealthCheckEventCommandHandlerTestsFixture()
        {
            HealthChecks = new List<HealthCheck>
            {
                EntityActivator.CreateInstance<HealthCheck>().Set(h => h.Id, 1),
                EntityActivator.CreateInstance<HealthCheck>().Set(h => h.Id, 2)
            };
            
            Command = new ReceiveProviderRelationshipsHealthCheckEventCommand(HealthChecks[1].Id);
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)).Options);
            
            Db.HealthChecks.AddRange(HealthChecks);
            Db.SaveChanges();
            
            Handler = new ReceiveProviderRelationshipsHealthCheckEventCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}