using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.UpdatePermissions;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.ProviderRelationships
{
    [TestFixture]
    [Parallelizable]
    internal class UpdatedPermissionsEventHandlerTests : FluentTest<UpdatedPermissionsEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingEvent_ThenShouldSendUpdatePermissionsCommand()
        {
            return RunAsync(f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object),
                f => f.ReadStoreMediator.Verify(x => x.Send(It.Is<UpdatePermissionsCommand>(p =>
                        p.Ukprn == f.Ukprn &&
                        p.AccountProviderLegalEntityId == f.AccountProviderLegalEntityId &&
                        p.GrantedOperations == f.GrantedOperations &&
                        p.Updated == f.Created &&
                        p.MessageId == f.MessageId), 
                    It.IsAny<CancellationToken>())));
        }
    }

    internal class UpdatedPermissionsEventHandlerTestsFixture
    {
        public string MessageId = "messageId";
        public UpdatedPermissionsEvent Message;
        public long AccountId = 1;
        public long AccountLegalEntityId = 2;
        public long AccountProviderId = 3;
        public long AccountProviderLegalEntityId = 4;
        public long Ukprn = 12345678;
        public Guid UserRef = Guid.NewGuid();
        public HashSet<Operation> GrantedOperations = new HashSet<Operation>();
        public DateTime Created = DateTime.Now.AddMinutes(-1);
        public Mock<IMessageHandlerContext> MessageHandlerContext;
        public Mock<IReadStoreMediator> ReadStoreMediator;
        public UpdatedPermissionsEventHandler Handler;

        public UpdatedPermissionsEventHandlerTestsFixture()
        {
            ReadStoreMediator = new Mock<IReadStoreMediator>();
            MessageHandlerContext = new Mock<IMessageHandlerContext>();
            MessageHandlerContext.Setup(x => x.MessageId).Returns(MessageId);
            
            Message = new UpdatedPermissionsEvent(AccountId, AccountLegalEntityId, AccountProviderId, AccountProviderLegalEntityId, Ukprn, UserRef, GrantedOperations, Created);
            Handler = new UpdatedPermissionsEventHandler(ReadStoreMediator.Object);
        }
    }
}