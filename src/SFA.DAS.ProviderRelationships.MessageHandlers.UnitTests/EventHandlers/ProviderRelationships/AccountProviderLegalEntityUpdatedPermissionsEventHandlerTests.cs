using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.ProviderRelationships
{
    [TestFixture]
    [Parallelizable]
    internal class AccountProviderLegalEntityUpdatedPermissionsEventHandlerTests : FluentTest<AccountProviderLegalEntityUpdatedPermissionsEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingEvent_ThenShouldSendUpdateRelationshipCommand()
        {
            return RunAsync(f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.ReadStoreMediator.Verify(x => x.Send(It.Is<UpdateRelationshipCommand>(p =>
                        p.Ukprn == f.Ukprn &&
                        p.AccountProviderId == f.AccountProviderId &&
                        p.AccountId == f.AccountId &&
                        p.AccountLegalEntityId == f.AccountLegalEntityId &&
                        p.Operations == f.Operations &&
                        p.Updated == f.Created &&
                        p.MessageId == f.MessageId
                        ), 
                    It.IsAny<CancellationToken>())));
        }
    }

    internal class AccountProviderLegalEntityUpdatedPermissionsEventHandlerTestsFixture
    {
        public string MessageId = "messageId";
        public UpdatedPermissionsEvent Message;
        public long Ukprn = 11111;
        public int AccountProviderId = 55555;
        public long AccountId = 333333;
        public long AccountLegalEntityId = 44444;
        public Guid UserRef = Guid.NewGuid();
        public HashSet<Operation> Operations = new HashSet<Operation>();
        public DateTime Created = DateTime.Now.AddMinutes(-1);

        public Mock<IMessageHandlerContext> MessageHandlerContext;
        public Mock<IReadStoreMediator> ReadStoreMediator;
        public AccountProviderLegalEntityUdatedPermissionsEventHandler Handler;

        public AccountProviderLegalEntityUpdatedPermissionsEventHandlerTestsFixture()
        {
            ReadStoreMediator = new Mock<IReadStoreMediator>();
            MessageHandlerContext = new Mock<IMessageHandlerContext>();
            MessageHandlerContext.Setup(x => x.MessageId).Returns(MessageId);
            
            Message = new UpdatedPermissionsEvent(AccountId, AccountLegalEntityId, AccountProviderId, Ukprn, UserRef, Operations, Created);

            Handler = new AccountProviderLegalEntityUdatedPermissionsEventHandler(ReadStoreMediator.Object);
        }
    }
}