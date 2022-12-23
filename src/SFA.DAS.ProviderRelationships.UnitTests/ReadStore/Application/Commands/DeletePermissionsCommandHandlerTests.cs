using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.DeletePermissions;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.ReadStore.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    internal class DeletePermissionsCommandHandlerTests : FluentTest<DeletePermissionsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenAccountProviderLegalEntityHasNotAlreadyBeenDeleted_ThenShouldDeleteAccountLegalEntity()
        {
            return TestAsync(
                f => f.Handle(),
                f =>
                {
                    f.AccountProviderLegalEntity.Operations.Should().BeEmpty();
                    f.AccountProviderLegalEntity.Deleted.Should().Be(f.Command.Deleted);
                });
        }
        
        [Test]
        public Task Handle_WhenAccountProviderLegalEntityHasAlreadyBeenDeleted_ThenShouldThrowException()
        {
            return TestExceptionAsync(
                f => f.SetAccountProviderLegalEntityDeletedBeforeCommand(), 
                f => f.Handle(), 
                (f, r) => r.Should().ThrowAsync<InvalidOperationException>());
        }
    }

    internal class DeletePermissionsCommandHandlerTestsFixture
    {
        public DeletePermissionsCommand Command { get; set; }
        public AccountProviderLegalEntity AccountProviderLegalEntity { get; set; }
        public IRequestHandler<DeletePermissionsCommand, Unit> Handler { get; set; }
        public Mock<IAccountProviderLegalEntitiesRepository> RelationshipsRepository { get; set; }
        public List<AccountProviderLegalEntity> AccountProviderLegalEntities { get; set; }
        public DateTime Now { get; set; }

        public DeletePermissionsCommandHandlerTestsFixture()
        {
            Now = DateTime.UtcNow;
            Command = new DeletePermissionsCommand(1, 12345678, Now.AddHours(-1), Guid.NewGuid().ToString());
            AccountProviderLegalEntity = DocumentActivator.CreateInstance<AccountProviderLegalEntity>().Set(r => r.AccountProviderLegalEntityId, 1).Set(r => r.Ukprn, Command.Ukprn);
            RelationshipsRepository = new Mock<IAccountProviderLegalEntitiesRepository>();
            
            AccountProviderLegalEntities = new List<AccountProviderLegalEntity>
            {
                AccountProviderLegalEntity,
                DocumentActivator.CreateInstance<AccountProviderLegalEntity>().Set(r => r.AccountProviderLegalEntityId, 2).Set(r => r.Ukprn, Command.Ukprn)
            };
            
            RelationshipsRepository.SetupInMemoryCollection(AccountProviderLegalEntities);
            
            Handler = new DeletePermissionsCommandHandler(RelationshipsRepository.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Command, CancellationToken.None);
        }

        public DeletePermissionsCommandHandlerTestsFixture SetAccountProviderLegalEntityDeletedAfterCommand()
        {
            AccountProviderLegalEntity.Set(ale => ale.Deleted, Now);
            
            return this;
        }

        public DeletePermissionsCommandHandlerTestsFixture SetAccountProviderLegalEntityDeletedBeforeCommand()
        {
            AccountProviderLegalEntity.Set(ale => ale.Deleted, Command.Deleted.AddHours(-1));
            
            return this;
        }
    }
}