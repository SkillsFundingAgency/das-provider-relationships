using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdatePermissions;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UpdatePermissionsCommandHandlerTests : FluentTest<UpdatePermissionsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenAccountProviderLegalEntityDoesNotExist_ThenShouldCreatePermissions()
        {
            return TestAsync(f => f.Handle(), f => f.Db.AccountProviderLegalEntities
                .SingleOrDefault(aple =>
                    aple.AccountProviderId == f.AccountProvider.Id &&
                    aple.AccountLegalEntityId == f.AccountLegalEntity.Id &&
                    aple.Permissions.All(p => f.Command.GrantedOperations.Contains(p.Operation)) &&
                    aple.Created >= f.Now &&
                    aple.Updated == null)
                .Should()
                .NotBeNull());
        }
        
        [Test]
        public Task Handle_WhenAccountProviderLegalEntityDoesNotExist_ThenShouldPublishUpdatedPermissionsEvent()
        {
            return TestAsync(
                f => f.Handle(), 
                f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                .And.Match<UpdatedPermissionsEvent>(e =>
                    e.AccountId == f.Account.Id &&
                    e.AccountLegalEntityId == f.AccountLegalEntity.Id &&
                    e.AccountProviderId == f.AccountProvider.Id &&
                    e.AccountProviderLegalEntityId == f.AccountProvider.AccountProviderLegalEntities.Single().Id &&
                    e.Ukprn == f.AccountProvider.ProviderUkprn &&
                    e.UserRef == f.User.Ref &&
                    e.GrantedOperations == f.Command.GrantedOperations &&
                    e.Updated == f.AccountProvider.AccountProviderLegalEntities.Single().Created));
        }
        
        [Test]
        public Task Handle_WhenAccountProviderLegalEntityExists_ThenShouldUpdatePermissions()
        {
            return TestAsync(
                f => f.SetAccountProviderLegalEntity(), 
                f => f.Handle(), 
                f => f.Db.AccountProviderLegalEntities
                    .SingleOrDefault(aple =>
                        aple.Id == f.AccountProviderLegalEntity.Id &&
                        aple.Permissions.All(p => f.Command.GrantedOperations.Contains(p.Operation)) &&
                        aple.Updated >= f.Now)
                    .Should()
                    .NotBeNull());
        }
        
        [Test]
        public Task Handle_WhenAccountProviderLegalEntityExists_ThenShouldPublishUpdatedPermissionsEvent()
        {
            return TestAsync(
                f => f.SetAccountProviderLegalEntity(), 
                f => f.Handle(), 
                f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
                    .And.Match<UpdatedPermissionsEvent>(e =>
                        e.AccountId == f.Account.Id &&
                        e.AccountLegalEntityId == f.AccountLegalEntity.Id &&
                        e.AccountProviderId == f.AccountProvider.Id &&
                        e.AccountProviderLegalEntityId == f.AccountProviderLegalEntity.Id &&
                        e.Ukprn == f.AccountProvider.ProviderUkprn &&
                        e.UserRef == f.User.Ref &&
                        e.GrantedOperations == f.Command.GrantedOperations &&
                        e.Updated == f.AccountProviderLegalEntity.Updated));
        }
    }

    public class UpdatePermissionsCommandHandlerTestsFixture
    {
        public ProviderRelationshipsDbContext Db { get; set; }
        public Account Account { get; set; }
        public AccountProvider AccountProvider { get; set; }
        public AccountLegalEntity AccountLegalEntity { get; set; }
        public User User { get; set; }
        public DateTime Now { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public UpdatePermissionsCommand Command { get; set; }
        public IRequestHandler<UpdatePermissionsCommand, Unit> Handler { get; set; }
        public AccountProviderLegalEntity AccountProviderLegalEntity { get; set; }
        
        public UpdatePermissionsCommandHandlerTestsFixture()
        {
            Now = DateTime.UtcNow;;
            UnitOfWorkContext = new UnitOfWorkContext();
            Db = new ProviderRelationshipsDbContext(
                new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Command = new UpdatePermissionsCommand(1, 2, 3, Guid.NewGuid(), new HashSet<Operation> { Operation.CreateCohort });
            Account = EntityActivator.CreateInstance<Account>()
                .Set(a => a.Id, Command.AccountId)
                .Set(a => a.Name, Guid.NewGuid().ToString())
                .Set(a => a.HashedId, Guid.NewGuid().ToString())
                .Set(a => a.PublicHashedId, Guid.NewGuid().ToString());
            AccountProvider = EntityActivator.CreateInstance<AccountProvider>()
                .Set(ap => ap.Id, Command.AccountProviderId)
                .Set(ap => ap.AccountId, Account.Id);
            AccountLegalEntity = EntityActivator.CreateInstance<AccountLegalEntity>()
                .Set(ale => ale.Id, Command.AccountLegalEntityId)
                .Set(ale => ale.Name, Guid.NewGuid().ToString())
                .Set(ale => ale.PublicHashedId, Guid.NewGuid().ToString())
                .Set(ale => ale.AccountId, Account.Id);
            User = EntityActivator.CreateInstance<User>()
                .Set(u => u.Ref, Command.UserRef)
                .Set(u => u.Email, Guid.NewGuid().ToString())
                .Set(u => u.FirstName, Guid.NewGuid().ToString())
                .Set(u => u.LastName, Guid.NewGuid().ToString());
            
            Db.Accounts.Add(Account);
            Db.AccountProviders.Add(AccountProvider);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.Users.Add(User);
            Db.SaveChanges();
            
            Handler = new UpdatePermissionsCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }

        public UpdatePermissionsCommandHandlerTestsFixture SetAccountProviderLegalEntity()
        {
            AccountProviderLegalEntity = EntityActivator.CreateInstance<AccountProviderLegalEntity>()
                .Set(aple => aple.Id, 4)
                .Set(aple => aple.AccountProviderId, AccountProvider.Id)
                .Set(aple => aple.AccountLegalEntityId, AccountLegalEntity.Id);
            
            Db.AccountProviderLegalEntities.Add(AccountProviderLegalEntity);
            
            Db.SaveChanges();
            
            return this;
        }
    }
}