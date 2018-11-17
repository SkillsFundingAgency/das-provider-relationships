using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [NonParallelizable]
    public class RemovedLegalEntityEventHandlerTests : FluentTest<RemovedLegalEntityEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenEventIsHandledChronologicallyAndAccountLegalEntityHasNotAlreadyBeenDeleted_ThenShouldDeleteAccountLegalEntity()
        {
            return RunAsync(f => f.Handle(), f => f.AccountLegalEntity.Deleted.Should().Be(f.Message.Created));
        }
        
        [Test]
        public Task Handle_WhenEventIsHandledNonChronologically_ThenShouldNotDeleteAccountLegalEntity()
        {
            return RunAsync(f => f.SetAccountLegalEntityDeletedAfterEvent(), f => f.Handle(), f => f.AccountLegalEntity.Deleted.Should().Be(f.Now));
        }
        
        [Test]
        public Task Handle_WhenEventIsHandledChronologicallyAndAccountLegalEntityHasAlreadyBeenDeleted_ThenShouldThrowException()
        {
            return RunAsync(f => f.SetAccountLegalEntityDeletedBeforeEvent(), f => f.Handle(), (f, r) => r.Should().Throw<InvalidOperationException>());
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
                AccountLegalEntityId = 1,
                Created = Now.AddHours(-1)
            };
            
            AccountLegalEntity = new AccountLegalEntityBuilder().WithId(Message.AccountLegalEntityId);

            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.SaveChanges();
        }

        public RemovedLegalEntityEventHandlerTestsFixture SetAccountLegalEntityDeletedAfterEvent()
        {
            AccountLegalEntity.SetPropertyTo(ale => ale.Deleted, Now);
            Db.SaveChanges();
            
            return this;
        }

        public RemovedLegalEntityEventHandlerTestsFixture SetAccountLegalEntityDeletedBeforeEvent()
        {
            AccountLegalEntity.SetPropertyTo(ale => ale.Deleted, Message.Created.AddHours(-1));
            Db.SaveChanges();
            
            return this;
        }
    }
}