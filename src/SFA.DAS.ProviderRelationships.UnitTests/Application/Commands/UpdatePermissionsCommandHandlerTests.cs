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
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class UpdatePermissionsCommandHandlerTests : FluentTest<UpdatePermissionsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenAccountProviderLegalEntityDoesNotExist_ThenShouldCreatePermissions()
        {
            return RunAsync(f => f.Handle(), f => f.Db.AccountProviderLegalEntities
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
            return RunAsync(f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
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
            return RunAsync(f => f.SetAccountProviderLegalEntity(), f => f.Handle(), f => f.Db.AccountProviderLegalEntities
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
            return RunAsync(f => f.SetAccountProviderLegalEntity(), f => f.Handle(), f => f.UnitOfWorkContext.GetEvents().SingleOrDefault().Should().NotBeNull()
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
        
        [Test]
        public Task Handle_WhenHandlingCreateOrUpdatePermissionsCommand_ThenShouldReturnAccountLegalEntityId()
        {
            return RunAsync(f => f.SetAccountProviderLegalEntity(), f => f.Handle(), (f, r) => r.Should().Be(f.AccountProviderLegalEntity.Id));
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
        public IRequestHandler<UpdatePermissionsCommand, long> Handler { get; set; }
        public AccountProviderLegalEntity AccountProviderLegalEntity { get; set; }
        
        public UpdatePermissionsCommandHandlerTestsFixture()
        {
            Now = DateTime.UtcNow;;
            UnitOfWorkContext = new UnitOfWorkContext();
            Db = new ProviderRelationshipsDbContext(new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Command = new UpdatePermissionsCommand(1, 2, 3, Guid.NewGuid(), new HashSet<Operation> { Operation.CreateCohort });
            Account = new AccountBuilder().WithId(Command.AccountId);
            AccountProvider = new AccountProviderBuilder().WithId(Command.AccountProviderId).WithAccountId(Account.Id);
            AccountLegalEntity = new AccountLegalEntityBuilder().WithId(Command.AccountLegalEntityId).WithAccountId(Account.Id);
            User = new UserBuilder().WithRef(Command.UserRef);
            
            Db.Accounts.Add(Account);
            Db.AccountProviders.Add(AccountProvider);
            Db.AccountLegalEntities.Add(AccountLegalEntity);
            Db.Users.Add(User);
            Db.SaveChanges();
            
            Handler = new UpdatePermissionsCommandHandler(new Lazy<ProviderRelationshipsDbContext>(() => Db));
        }

        public Task<long> Handle()
        {
            return Handler.Handle(Command, CancellationToken.None);
        }

        public UpdatePermissionsCommandHandlerTestsFixture SetAccountProviderLegalEntity()
        {
            AccountProviderLegalEntity = new AccountProviderLegalEntityBuilder()
                .WithId(4)
                .WithAccountProviderId(AccountProvider.Id)
                .WithAccountLegalEntityId(AccountLegalEntity.Id);
            
            AccountProvider.SetPropertyTo(ap => ap.AccountProviderLegalEntities, new List<AccountProviderLegalEntity> { AccountProviderLegalEntity });
            
            Db.SaveChanges();
            
            return this;
        }
    }
}