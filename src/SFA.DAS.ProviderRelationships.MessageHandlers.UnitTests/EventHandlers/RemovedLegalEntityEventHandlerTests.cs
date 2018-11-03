using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    [TestFixture]
    [NonParallelizable]
    public class RemovedLegalEntityEventHandlerTests : FluentTest<RemovedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingRemovedLegalEntityEvent_ThenShouldDeleteAccountLegalEntity()
        {
            return RunAsync(f => f.Handle(), f => f.Db.AccountLegalEntities.Should().BeEmpty());
        }
    }

    public class RemovedLegalEntityEventHandlerTestsFixture : EventHandlerTestsFixture<RemovedLegalEntityEvent>
    {
        public AccountLegalEntity AccountLegalEntity { get; set; }

        public RemovedLegalEntityEventHandlerTestsFixture()
            : base(db => new RemovedLegalEntityEventHandler(db))
        {
            Message = new RemovedLegalEntityEvent
            {
                AccountLegalEntityId = 456,
                Created = DateTime.UtcNow
            };
            
            AccountLegalEntity = new AccountLegalEntityBuilder()
                .WithId(Message.AccountLegalEntityId)
                .Build();

            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.SaveChanges();
        }
    }
}