using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.UpdatePermissions;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.UnitTests.Builders;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.ReadStore.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    internal class UpdatePermissionsCommandHandlerTests : FluentTest<UpdatePermissionsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenDocumentDoesNotExist_ThenShouldCreateDocument()
        {
            return TestAsync(f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Add(It.Is<AccountProviderLegalEntity>(p =>
                        p.Ukprn == f.Ukprn &&
                        p.AccountProviderId == f.AccountProviderId &&
                        p.AccountId == f.AccountId &&
                        p.AccountLegalEntityId == f.AccountLegalEntityId &&
                        p.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
                        p.Operations.Equals(f.Operations) &&
                        p.Created == f.Updated &&
                        p.Updated == null
                    ), null,
                    It.IsAny<CancellationToken>())));
        }
        
        [Test]
        public Task Handle_WhenDocumentExists_ThenShouldUpdateDocument()
        {
            return TestAsync(f => f.FindMatchingRelationship(),
                f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<AccountProviderLegalEntity>(p =>
                        p.Ukprn == f.Ukprn &&
                        p.AccountProviderId == f.AccountProviderId &&
                        p.AccountId == f.AccountId &&
                        p.AccountLegalEntityId == f.AccountLegalEntityId &&
                        p.Operations.Equals(f.Operations) &&
                        p.Updated == f.Updated
                    ), null,
                    It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenCommandIsNotDuplicateAndIsHandledChronologically_ThenShouldAddUpdateMessageIdToOutbox()
        {
            return TestAsync(f => f.FindMatchingRelationship(),
                f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<AccountProviderLegalEntity>(p =>
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Count(o => o.MessageId == f.UpdateMessageId) == 1
                    ), null,
                    It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenCommandIsDuplicate_ThenShouldSimplyIgnoreTheUpdate()
        {
            return TestAsync(f => f.FindMatchingRelationshipWithUpdateMessageAlreadyProcessed(),
                f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<AccountProviderLegalEntity>(p =>
                        p.Operations.Contains(Operation.CreateCohort) == false
                    ), null,
                    It.IsAny<CancellationToken>())));
        }

        [Test]
        public Task Handle_WhenCommandIsNotHandledChronologically_ThenShouldSimplySwallowTheMessageAndAddItToTheOutbox()
        {
            return TestAsync(f => f.FindMatchingRelationshipWhichWasUpdatedLaterThanNewMessage(),
                f => f.Handler.Handle(f.Command, CancellationToken.None),
                f => f.RelationshipsRepository.Verify(x => x.Update(It.Is<AccountProviderLegalEntity>(p =>
                        p.Operations.Contains(Operation.CreateCohort) == false &&
                        p.OutboxData.Count() == 2 &&
                        p.OutboxData.Count(o => o.MessageId == f.UpdateMessageId) == 1
                    ), null,
                    It.IsAny<CancellationToken>())));
        }
    }

    internal class UpdatePermissionsCommandHandlerTestsFixture
    {
        public string CreateMessageId = "createMessageId";
        public string UpdateMessageId = "updateMessageId";
        public long Ukprn = 11111;
        public int AccountProviderId = 55555;
        public long AccountId = 333333;
        public long AccountLegalEntityId = 44444;
        public long AccountProviderLegalEntityId = 6666;
        public HashSet<Operation> Operations = new HashSet<Operation> { Operation.CreateCohort };
        public DateTime Created = DateTime.Now.AddMinutes(-10);
        public DateTime Updated = DateTime.Now.AddMinutes(-1);
        public Mock<IAccountProviderLegalEntitiesRepository> RelationshipsRepository;
        public List<AccountProviderLegalEntity> Relationships;
        public UpdatePermissionsCommand Command;
        public IRequestHandler<UpdatePermissionsCommand> Handler;
        
        public UpdatePermissionsCommandHandlerTestsFixture()
        {
            RelationshipsRepository = new Mock<IAccountProviderLegalEntitiesRepository>();
            Relationships = new List<AccountProviderLegalEntity>();

            RelationshipsRepository.SetupInMemoryCollection(Relationships);

            Handler = new UpdatePermissionsCommandHandler(RelationshipsRepository.Object, Mock.Of<ILogger<UpdatePermissionsCommandHandler>>());
            Command = new UpdatePermissionsCommand(AccountId, AccountLegalEntityId, AccountProviderId, AccountProviderLegalEntityId, Ukprn, Operations, Updated, UpdateMessageId);
        }

        public UpdatePermissionsCommandHandlerTestsFixture FindMatchingRelationship()
        {
            Relationships.Add(CreateBasicRelationship());
            
            return this;
        }

        public UpdatePermissionsCommandHandlerTestsFixture FindMatchingRelationshipWhichWasDeletedEarlierThanNewMessage()
        {
            Relationships.Add(CreateBasicRelationship().Set(r => r.Deleted, Updated.AddHours(-1)));
            
            return this;
        }

        public UpdatePermissionsCommandHandlerTestsFixture FindMatchingRelationshipWhichWasDeletedLaterThanNewMessage()
        {
            Relationships.Add(CreateBasicRelationship().Set(r => r.Deleted, Updated.AddHours(1)));
            
            return this;
        }

        public UpdatePermissionsCommandHandlerTestsFixture FindMatchingRelationshipWhichWasUpdatedLaterThanNewMessage()
        {
            Relationships.Add(CreateBasicRelationship().Set(r => r.Updated, Updated.AddHours(1)));
            
            return this;
        }

        public UpdatePermissionsCommandHandlerTestsFixture FindMatchingRelationshipWithUpdateMessageAlreadyProcessed()
        {
            Relationships.Add(CreateBasicRelationship().Add(r => r.OutboxData, new OutboxMessage(UpdateMessageId, Updated)));
            
            return this;
        }

        private AccountProviderLegalEntity CreateBasicRelationship()
        {
            return DocumentActivator.CreateInstance<AccountProviderLegalEntity>()
                .Set(r => r.Ukprn, Ukprn)
                .Set(r => r.AccountProviderId, AccountProviderId)
                .Set(r => r.AccountId, AccountId)
                .Set(r => r.AccountLegalEntityId, AccountLegalEntityId)
                .Set(r => r.AccountProviderLegalEntityId, AccountProviderLegalEntityId)
                .Add(r => r.OutboxData, new OutboxMessage(CreateMessageId, Created));
        }
    }
}
