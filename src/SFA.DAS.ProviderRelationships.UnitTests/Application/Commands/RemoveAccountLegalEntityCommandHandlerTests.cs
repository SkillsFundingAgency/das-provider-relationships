using System;
using System.Collections.Generic;
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
        public Task Handle_WhenAccountLegalEntityHasNotAlreadyBeenDeleted_ThenShouldDeleteAccountLegalEntity()
        {
            return RunAsync(f => f.Handle(), f => f.AccountLegalEntity.Deleted.Should().Be(f.Command.Removed));
        }
        
        [Test]
        public Task Handle_WhenAccountLegalEntityHasNotAlreadyBeenDeletedAndAccountProviderLegalEntitiesExist_ThenShouldDeleteAccountProviderLegalEntities()
        {
            return RunAsync(f => f.SetAccountProviderLegalEntities(), f => f.Handle(), f => f.AccountLegalEntity.AccountProviderLegalEntities.Should().BeEmpty());
        }
        
        [Test]
        public Task Handle_WhenAccountLegalEntityHasNotAlreadyBeenDeletedAndAccountProviderLegalEntitiesExist_ThenShouldPublishDeletedPermissionsEvents()
        {
            return RunAsync(f => f.SetAccountProviderLegalEntities(), f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().Should().AllBeOfType<DeletedPermissionsEvent>()
                .And.BeEquivalentTo(f.AccountProviderLegalEntities.Select(aple => new DeletedPermissionsEvent(aple.Id, f.AccountProvider.ProviderUkprn, f.Command.Removed))));
        }
        
        [Test]
        public Task Handle_WhenAccountLegalEntityHasNotAlreadyBeenDeletedAndAccountProviderLegalEntitiesDoNotExist_ThenShouldNotPublishDeletedPermissionsEvent()
        {
            return RunAsync(f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().BeNull());
        }
        
        [Test]
        public Task Handle_WhenAccountLegalEntityHasAlreadyBeenDeleted_ThenShouldThrowException()
        {
            return RunAsync(f => f.SetAccountLegalEntityDeletedBeforeCommand(), f => f.Handle(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }
    }

    public class RemoveAccountLegalEntityCommandHandlerTestsFixture
    {
        public Account Account { get; set; }
        public AccountLegalEntity AccountLegalEntity { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public List<AccountProviderLegalEntity> AccountProviderLegalEntities { get; set; }
        public RemoveAccountLegalEntityCommand Command { get; set; }
        public IRequestHandler<RemoveAccountLegalEntityCommand, Unit> Handler { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public DateTime Now { get; set; }

        public RemoveAccountLegalEntityCommandHandlerTestsFixture()
        {
            Now = DateTime.UtcNow;
            Account = EntityActivator.CreateInstance<Account>().Set(a => a.Id, 1);
            AccountLegalEntity = EntityActivator.CreateInstance<AccountLegalEntity>().Set(ale => ale.Id, 2).Set(ale => ale.AccountId, Account.Id);
            AccountProvider = EntityActivator.CreateInstance<AccountProvider>().Set(ap => ap.Id, 3).Set(ap => ap.AccountId, Account.Id).Set(ap => ap.ProviderUkprn, 12345678);
            Command = new RemoveAccountLegalEntityCommand(Account.Id, AccountLegalEntity.Id, Now.AddHours(-1));
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

            Db.Accounts.Add(Account);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.AccountProviders.Add(AccountProvider);
            Db.SaveChanges();

            Handler = new RemoveAccountLegalEntityCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
            UnitOfWorkContext = new UnitOfWorkContext();
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public RemoveAccountLegalEntityCommandHandlerTestsFixture SetAccountLegalEntityDeletedBeforeCommand()
        {
            AccountLegalEntity.Set(ale => ale.Deleted, Command.Removed.AddHours(-1));
            Db.SaveChanges();
            
            return this;
        }

        public RemoveAccountLegalEntityCommandHandlerTestsFixture SetAccountProviderLegalEntities()
        {
            AccountProviderLegalEntities = new List<AccountProviderLegalEntity>
            {
                EntityActivator.CreateInstance<AccountProviderLegalEntity>().Set(aple => aple.Id, 4).Set(aple => aple.AccountProvider, AccountProvider),
                EntityActivator.CreateInstance<AccountProviderLegalEntity>().Set(aple => aple.Id, 5).Set(aple => aple.AccountProvider, AccountProvider),
            };
            
            AccountLegalEntity.AddRange(ale => ale.AccountProviderLegalEntities, AccountProviderLegalEntities);
            Db.SaveChanges();
            
            return this;
        }
    }
}