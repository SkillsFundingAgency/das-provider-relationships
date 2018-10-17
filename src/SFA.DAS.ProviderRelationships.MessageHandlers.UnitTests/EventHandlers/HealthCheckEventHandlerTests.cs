using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    public class HealthCheckEventHandlerTests : FluentTest<HealthCheckEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAHealthCheckEvent_ThenShouldUpdateHealthCheck()
        {
            return RunAsync(f => f.Handle(), f => f.HealthChecks[1].ReceivedProviderRelationshipsEvent.Should().NotBeNull());
        }
    }

    public class HealthCheckEventHandlerTestsFixture
    {
        public IHandleMessages<HealthCheckEvent> Handler { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public List<HealthCheck> HealthChecks { get; set; }

        public HealthCheckEventHandlerTestsFixture()
        {
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

            HealthChecks = new List<HealthCheck>
            {
                new HealthCheckBuilder().WithId(1).Build(),
                new HealthCheckBuilder().WithId(2).Build()
            };
            
            Db.HealthChecks.AddRange(HealthChecks);
            Db.SaveChanges();
            
            Handler = new HealthCheckEventHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public Task Handle()
        {
            return Handler.Handle(new HealthCheckEvent { Id = HealthChecks[1].Id }, null);
        }
    }
}