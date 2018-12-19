using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers.ProviderRelationships
{
    [TestFixture]
    [Parallelizable]
    public class AddedAccountProviderEventHandlerTests : FluentTest<AddedAccountProviderEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingAddedAccountProviderEvent_ThenShouldSendAuditCommand()
        {
            return RunAsync(f => f.Handler.Handle(f.Message, f.MessageHandlerContext.Object), f => f.Mediator.Verify(m => m.Send(It.Is<AddedAccountProviderEventAuditCommand>(c =>
                c.AccountProviderId == f.AccountProviderId &&
                c.AccountId == f.AccountId &&
                c.ProviderUkprn == f.ProviderUkprn &&
                c.UserRef == f.UserRef &&
                c.Added == f.Added
            ), CancellationToken.None), Times.Once));
        }
    }

    public class AddedAccountProviderEventHandlerTestsFixture
    {
        public AddedAccountProviderEvent Message { get; set; }
        public string MessageId { get; set; }
        public Mock<IMessageHandlerContext> MessageHandlerContext { get; set; }
        public IHandleMessages<AddedAccountProviderEvent> Handler { get; set; }
        public Mock<IMediator> Mediator { get; set; }

        public long AccountProviderId { get; set; }
        public long AccountId { get; set; }
        public long ProviderUkprn { get; set; }
        public Guid UserRef { get; set; }
        public DateTime Added { get; set; }

        public AddedAccountProviderEventHandlerTestsFixture()
        {
            AccountProviderId = 112;
            AccountId = 114;
            ProviderUkprn = 28933829;
            UserRef = Guid.NewGuid();
            Added = DateTime.Parse("2018-11-11");

            Message = new AddedAccountProviderEvent(AccountProviderId, AccountId, ProviderUkprn, UserRef, Added);
            MessageId = Guid.NewGuid().ToString();
            MessageHandlerContext = new Mock<IMessageHandlerContext>();
            Mediator = new Mock<IMediator>();

            MessageHandlerContext.Setup(c => c.MessageId).Returns(MessageId);

            Handler = new AddedAccountProviderEventHandler(Mediator.Object);
        }
    }
}
