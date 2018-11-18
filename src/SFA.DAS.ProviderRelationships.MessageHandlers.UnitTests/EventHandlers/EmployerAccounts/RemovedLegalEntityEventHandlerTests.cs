using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
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
        public Task Handle_WhenEventIsHandledChronologically_ThenShouldPublishDeletedAccountLegalEntityEvent()
        {
            return RunAsync(f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                .And.Match<DeletedAccountLegalEntityEvent>(e =>
                    e.AccountLegalEntityId == f.AccountLegalEntity.Id &&
                    e.AccountId == f.AccountLegalEntity.AccountId &&
                    e.Created == f.AccountLegalEntity.Deleted));
        }
        
        [Test]
        public Task Handle_WhenEventIsHandledChronologicallyAndAccountLegalEntityHasAlreadyBeenDeleted_ThenShouldThrowException()
        {
            return RunAsync(f => f.SetAccountLegalEntityDeletedBeforeEvent(), f => f.Handle(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }
    }

    public class RemovedLegalEntityEventHandlerTestsFixture : EventHandlerTestsFixture<RemovedLegalEntityEvent>
    {
        public Account Account { get; set; }
        public AccountLegalEntity AccountLegalEntity { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }

        public RemovedLegalEntityEventHandlerTestsFixture()
            : base(db => new RemovedLegalEntityEventHandler(db))
        {
            Message = new RemovedLegalEntityEvent
            {
                AccountId = 1,
                AccountLegalEntityId = 2,
                Created = Now.AddHours(-1)
            };

            Account = new AccountBuilder().WithId(Message.AccountId);
            AccountLegalEntity = new AccountLegalEntityBuilder().WithId(Message.AccountLegalEntityId).WithAccountId(Account.Id);

            Db.Accounts.Add(Account);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.SaveChanges();

            UnitOfWorkContext = new UnitOfWorkContext();
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