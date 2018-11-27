using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.ReadStore.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    internal class DeletePermissionsCommandHandlerTests : FluentTest<DeletePermissionsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenRelationshipHasNotAlreadyBeenDeleted_ThenShouldDeleteAccountLegalEntity()
        {
            return RunAsync(f => f.Handle(), f => f.Relationship.Deleted.Should().Be(f.Command.Deleted));
        }
        
        [Test]
        public Task Handle_WhenRelationshipHasAlreadyBeenDeleted_ThenShouldThrowException()
        {
            return RunAsync(f => f.SetRelationshipDeletedBeforeCommand(), f => f.Handle(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }
    }

    internal class DeletePermissionsCommandHandlerTestsFixture
    {
        public DeletePermissionsCommand Command { get; set; }
        public Relationship Relationship { get; set; }
        public IReadStoreRequestHandler<DeletePermissionsCommand, Unit> Handler { get; set; }
        public Mock<IRelationshipsRepository> RelationshipsRepository { get; set; }
        public List<Relationship> Relationships { get; set; }
        public DateTime Now { get; set; }

        public DeletePermissionsCommandHandlerTestsFixture()
        {
            Now = DateTime.UtcNow;
            Command = new DeletePermissionsCommand(1, 12345678, Now.AddHours(-1), Guid.NewGuid().ToString());
            Relationship = DocumentActivator.CreateInstance<Relationship>().Set(r => r.AccountProviderLegalEntityId, 1).Set(r => r.Ukprn, Command.Ukprn);
            RelationshipsRepository = new Mock<IRelationshipsRepository>();
            
            Relationships = new List<Relationship>
            {
                Relationship,
                DocumentActivator.CreateInstance<Relationship>().Set(r => r.AccountProviderLegalEntityId, 2).Set(r => r.Ukprn, Command.Ukprn)
            };
            
            RelationshipsRepository.SetupInMemoryCollection(Relationships);
            
            Handler = new DeletePermissionsCommandHandler(RelationshipsRepository.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Command, CancellationToken.None);
        }

        public DeletePermissionsCommandHandlerTestsFixture SetRelationshipDeletedAfterCommand()
        {
            Relationship.Set(ale => ale.Deleted, Now);
            
            return this;
        }

        public DeletePermissionsCommandHandlerTestsFixture SetRelationshipDeletedBeforeCommand()
        {
            Relationship.Set(ale => ale.Deleted, Command.Deleted.AddHours(-1));
            
            return this;
        }
    }
}