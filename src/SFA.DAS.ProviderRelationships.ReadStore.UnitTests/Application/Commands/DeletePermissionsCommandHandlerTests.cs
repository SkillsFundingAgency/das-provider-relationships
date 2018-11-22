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
        public Task Handle_WhenCommandIsHandledChronologicallyAndRelationshipHasNotAlreadyBeenDeleted_ThenShouldDeleteAccountLegalEntity()
        {
            return RunAsync(f => f.Handle(), f => f.Relationship.Deleted.Should().Be(f.Command.Deleted));
        }
        
        [Test]
        public Task Handle_WhenCommandIsHandledNonChronologically_ThenShouldNotDeleteRelationship()
        {
            return RunAsync(f => f.SetRelationshipDeletedAfterCommand(), f => f.Handle(), f => f.Relationship.Deleted.Should().Be(f.Now));
        }
        
        [Test]
        public Task Handle_WhenCommandIsHandledChronologicallyAndRelationshipHasAlreadyBeenDeleted_ThenShouldThrowException()
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
            Relationship = new RelationshipBuilder().WithAccountProviderLegalEntityId(1).WithUkprn(Command.Ukprn);
            RelationshipsRepository = new Mock<IRelationshipsRepository>();
            
            Relationships = new List<Relationship>
            {
                Relationship,
                new RelationshipBuilder().WithAccountProviderLegalEntityId(2).WithUkprn(Command.Ukprn)
            };
            
            RelationshipsRepository.SetupCreateQueryToReturn(Relationships);
            
            Handler = new DeletePermissionsCommandHandler(RelationshipsRepository.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Command, CancellationToken.None);
        }

        public DeletePermissionsCommandHandlerTestsFixture SetRelationshipDeletedAfterCommand()
        {
            Relationship.SetPropertyTo(ale => ale.Deleted, Now);
            
            return this;
        }

        public DeletePermissionsCommandHandlerTestsFixture SetRelationshipDeletedBeforeCommand()
        {
            Relationship.SetPropertyTo(ale => ale.Deleted, Command.Deleted.AddHours(-1));
            
            return this;
        }
    }
}