using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class RemoveAccountLegalEntityCommandHandlerTests : FluentTest<RemoveAccountLegalEntityCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenCommandIsHandledChronologicallyAndAccountLegalEntityHasNotAlreadyBeenDeleted_ThenShouldDeleteAccountLegalEntity()
        {
            return RunAsync(f => f.Handle(), f => f.AccountLegalEntity.Deleted.Should().Be(f.Command.Created));
        }
        
        [Test]
        public Task Handle_WhenCommandIsHandledNonChronologically_ThenShouldNotDeleteAccountLegalEntity()
        {
            return RunAsync(f => f.SetAccountLegalEntityDeletedAfterCommand(), f => f.Handle(), f => f.AccountLegalEntity.Deleted.Should().Be(f.Now));
        }
        
        [Test]
        public Task Handle_WhenCommandIsHandledChronologically_ThenShouldPublishDeletedAccountLegalEntityEvent()
        {
            return RunAsync(f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                .And.Match<DeletedAccountLegalEntityEvent>(e =>
                    e.AccountLegalEntityId == f.AccountLegalEntity.Id &&
                    e.AccountId == f.AccountLegalEntity.AccountId &&
                    e.Deleted == f.AccountLegalEntity.Deleted));
        }
        
        [Test]
        public Task Handle_WhenEventIsHandledChronologicallyAndAccountLegalEntityHasAlreadyBeenDeleted_ThenShouldThrowException()
        {
            return RunAsync(f => f.SetAccountLegalEntityDeletedBeforeCommand(), f => f.Handle(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }
    }

    public class RemoveAccountLegalEntityCommandHandlerTestsFixture
    {
        public AccountLegalEntity AccountLegalEntity { get; set; }
        public Account Account { get; set; }
        public RemoveAccountLegalEntityCommand Command { get; set; }
        public IRequestHandler<RemoveAccountLegalEntityCommand, Unit> Handler { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public DateTime Now { get; set; }

        public RemoveAccountLegalEntityCommandHandlerTestsFixture()
        {
            Now = DateTime.UtcNow;
            Account = new AccountBuilder().WithId(1);
            AccountLegalEntity = new AccountLegalEntityBuilder().WithId(2).WithAccountId(Account.Id);
            Command = new RemoveAccountLegalEntityCommand(Account.Id, AccountLegalEntity.Id, Now.AddHours(-1));
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

            Db.Accounts.Add(Account);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.SaveChanges();

            Handler = new RemoveAccountLegalEntityCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
            UnitOfWorkContext = new UnitOfWorkContext();
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public RemoveAccountLegalEntityCommandHandlerTestsFixture SetAccountLegalEntityDeletedAfterCommand()
        {
            AccountLegalEntity.SetPropertyTo(ale => ale.Deleted, Now);
            Db.SaveChanges();
            
            return this;
        }

        public RemoveAccountLegalEntityCommandHandlerTestsFixture SetAccountLegalEntityDeletedBeforeCommand()
        {
            AccountLegalEntity.SetPropertyTo(ale => ale.Deleted, Command.Created.AddHours(-1));
            Db.SaveChanges();
            
            return this;
        }
    }
}