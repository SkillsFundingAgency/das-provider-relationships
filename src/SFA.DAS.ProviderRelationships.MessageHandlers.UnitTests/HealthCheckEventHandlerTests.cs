using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Messages;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests;
using SFA.DAS.Testing;
using SFA.DAS.Testing.EntityFramework;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
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
        public Mock<IProviderRelationshipsDbContext> Db { get; set; }
        public List<HealthCheck> HealthChecks { get; set; }

        public HealthCheckEventHandlerTestsFixture()
        {
            Db = new Mock<IProviderRelationshipsDbContext>();

            HealthChecks = new List<HealthCheck>
            {
                new HealthCheckBuilder().WithId(1).Build(),
                new HealthCheckBuilder().WithId(2).Build()
            };

            Db.Setup(d => d.HealthChecks).Returns(new DbSetStub<HealthCheck>(HealthChecks));

            Handler = new HealthCheckEventHandler(new Lazy<IProviderRelationshipsDbContext>(() => Db.Object));
        }

        public Task Handle()
        {
            return Handler.Handle(new HealthCheckEvent { Id = HealthChecks[1].Id }, null);
        }
    }
}