using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SFA.DAS.Encoding;
using SFA.DAS.ProviderRelationships.Application.Commands.RevokePermissions;
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
    public class RevokePermissionsCommandHandlerTests : FluentTest<RevokePermissionsCommandHandlerTestsFixture>
    {
        [Test]
        public void WhenPublicHashedIdIsInvalid_ThenShouldThrowKeyNotFoundException()
        {
            var f = new RevokePermissionsCommandHandlerTestsFixture();
            f.RevokePermissionsCommand = new RevokePermissionsCommand(
                ukprn: f.RevokePermissionsCommand.Ukprn,
                accountLegalEntityPublicHashedId: "DoesNotExist",
                operationsToRevoke: f.RevokePermissionsCommand.OperationsToRevoke);
            f.EncodingService
                .Setup(x =>
                    x.Decode(f.RevokePermissionsCommand.AccountLegalEntityPublicHashedId, EncodingType.PublicAccountLegalEntityId))
                .Throws<KeyNotFoundException>();

            Assert.ThrowsAsync<KeyNotFoundException>(() => f.Handler.Handle(f.RevokePermissionsCommand, CancellationToken.None));
        }
                

        [Test]
        public Task WhenUkPrnIsInvalid_ThenShouldDoNothing() =>
            RunAsync(
                arrange: f =>
                {
                    f.RevokePermissionsCommand = new RevokePermissionsCommand(
                        ukprn: 0,
                        accountLegalEntityPublicHashedId: f.RevokePermissionsCommand.AccountLegalEntityPublicHashedId,
                        operationsToRevoke: f.RevokePermissionsCommand.OperationsToRevoke);
                },
                act: async f =>
                {
                    await f.Handler.Handle(f.RevokePermissionsCommand, CancellationToken.None);
                },
                assert: f =>
                {
                    IEnumerable<object> events = f.UnitOfWorkContext.GetEvents();
                    events.Should().BeEmpty();
                });

        [Test]
        [Parallelizable]
        public Task WhenExecuted_ThenShouldRemoveMatchingOperationsOnly() =>
            RunAsync(
                act: async f =>
                {
                    await f.Handler.Handle(f.RevokePermissionsCommand, CancellationToken.None);
                },
                assert: f =>
                {
                    f.AccountProviderLegalEntity
                        .Permissions
                        .Select(x => x.Operation)
                        .Should()
                        .BeEquivalentTo(new[] { Operation.CreateCohort });
                }
            );

        [Test]
        [Parallelizable]
        public Task WhenExecuted_ThenShouldPublishEvents() =>
            RunAsync(
                act: async f =>
                {
                    await f.Handler.Handle(f.RevokePermissionsCommand, CancellationToken.None);
                },
                assert: f =>
                {
                    IEnumerable<object> events = f.UnitOfWorkContext.GetEvents();
                    events.Should().HaveCount(1);

                    var firstEvent = events
                        .OfType<UpdatedPermissionsEvent>()
                        .Single();
                    firstEvent.AccountId.Should().Be(f.Account.Id);
                    firstEvent.AccountLegalEntityId.Should().Be(f.AccountLegalEntity.Id);
                    firstEvent.AccountProviderId.Should().Be(f.AccountProvider.Id);
                    firstEvent.AccountProviderLegalEntityId.Should().Be(f.AccountProviderLegalEntity.Id);
                    firstEvent.Ukprn.Should().Be(f.Provider.Ukprn);
                    firstEvent.UserRef.Should().Be(null);
                }
            );

        [Test]
        [Parallelizable]
        public Task WhenRevokedPermissionDidNotExist_ThenShouldNotPublishAnyEvents() =>
            RunAsync(
                arrange: f =>
                {
                    f.RevokePermissionsCommand = new RevokePermissionsCommand(
                        ukprn: f.RevokePermissionsCommand.Ukprn,
                        accountLegalEntityPublicHashedId: f.RevokePermissionsCommand.AccountLegalEntityPublicHashedId,
                        operationsToRevoke: new[] { (Operation)short.MinValue }
                        );
                },
                act: async f =>
                {
                    await f.Handler.Handle(f.RevokePermissionsCommand, CancellationToken.None);
                },
                assert: f =>
                {
                    IEnumerable<object> events = f.UnitOfWorkContext.GetEvents();
                    events.Should().BeEmpty();
                }
            );

    }

    public class RevokePermissionsCommandHandlerTestsFixture
    {
        public RevokePermissionsCommand RevokePermissionsCommand;
        public IRequestHandler<RevokePermissionsCommand, Unit> Handler { get; set; }
        public ProviderRelationshipsDbContext Db { get; set; }
        public IUnitOfWorkContext UnitOfWorkContext { get; set; }
        public Mock<IEncodingService> EncodingService { get; set; }

        public Account Account;
        public Provider Provider;
        public User User;
        public AccountLegalEntity AccountLegalEntity;
        public AccountProvider AccountProvider;
        public AccountProviderLegalEntity AccountProviderLegalEntity;

        public RevokePermissionsCommandHandlerTestsFixture()
        {
            UnitOfWorkContext = new UnitOfWorkContext();

            CreateDb();
            CreateDefaultEntities();

            EncodingService = new Mock<IEncodingService>();
            EncodingService
                .Setup(x => x.Decode(AccountLegalEntity.PublicHashedId, EncodingType.PublicAccountLegalEntityId))
                .Returns(AccountLegalEntity.Id);

            var lazyDb = new Lazy<ProviderRelationshipsDbContext>(() => Db);
            Handler = new RevokePermissionsCommandHandler(lazyDb, EncodingService.Object);

            RevokePermissionsCommand = new RevokePermissionsCommand(
                ukprn: 299792458,
                accountLegalEntityPublicHashedId: "ALE1",
                operationsToRevoke: new[] { Operation.Recruitment });

            UnitOfWorkContext = new UnitOfWorkContext();
        }

        private void CreateDb()
        {
            var optionsBuilder =
                new DbContextOptionsBuilder<ProviderRelationshipsDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .ConfigureWarnings(warnings =>
                        warnings.Throw(RelationalEventId.QueryClientEvaluationWarning)
                    );
            Db = new ProviderRelationshipsDbContext(optionsBuilder.Options);
        }

        private void CreateDefaultEntities()
        {
            Account = new Account(
                id: 1,
                hashedId: "HashedId",
                publicHashedId: "PublicHashedId",
                name: "Account",
                created: DateTime.UtcNow);
            Db.Add(Account);

            AccountLegalEntity = new AccountLegalEntity(
                account: Account,
                id: 12345,
                publicHashedId: "ALE1",
                name: "Account legal entity 1",
                created: DateTime.UtcNow);
            Db.Add(AccountLegalEntity);

            Provider = EntityActivator
                .CreateInstance<Provider>()
                .Set(x => x.Ukprn, 299792458);
            Db.Add(Provider);

            User = new User(Guid.NewGuid(), "me@home.com", "Bill", "Gates");
            Db.Add(User);

            AccountProvider = new AccountProvider(Account, Provider, User, null);
            AccountProvider.Set(x => x.Id, 23);
            Db.Add(AccountProvider);

            AccountProviderLegalEntity = new AccountProviderLegalEntity(
                accountProvider: AccountProvider,
                accountLegalEntity: AccountLegalEntity,
                user: User,
                grantedOperations: new HashSet<Operation>(new[] { Operation.CreateCohort, Operation.Recruitment }));
            AccountProviderLegalEntity.Set(x => x.Id, 34);
            Db.Add(AccountProviderLegalEntity);

            Db.SaveChanges();
        }
    }
}
