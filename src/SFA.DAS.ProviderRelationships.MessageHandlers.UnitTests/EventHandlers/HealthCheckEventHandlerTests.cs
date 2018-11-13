using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
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

    public class HealthCheckEventHandlerTestsFixture : EventHandlerTestsFixture<HealthCheckEvent>
    {
        public List<HealthCheck> HealthChecks { get; set; }

        public HealthCheckEventHandlerTestsFixture()
            : base(db => new HealthCheckEventHandler(db))
        {
            HealthChecks = new List<HealthCheck>
            {
                new HealthCheckBuilder().WithId(1),
                new HealthCheckBuilder().WithId(2)
            };
            
            Db.HealthChecks.AddRange(HealthChecks);
            Db.SaveChanges();
        }

        public override Task Handle()
        {
            return Handler.Handle(new HealthCheckEvent(HealthChecks[1].Id, DateTime.MinValue), null);
        }
    }
}