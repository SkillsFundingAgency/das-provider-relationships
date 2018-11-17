using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    public class ChangedAccountNameEventHandlerTests : FluentTest<ChangedAccountNameEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingChangedAccountNameEvent_ThenShouldSendBatchUpdateRelationshipAccountNamesCommands()
        {
            return RunAsync(f => f.Handle(), f => f.MessageHandlerContext.SentMessages
                .Select(m => m.Message)
                .Cast<BatchUpdateRelationshipAccountNamesCommand>()
                .Should()
                .BeEquivalentTo(f.Ukprns.Select(u => new
                {
                    Ukprn = u,
                    f.Message.AccountId,
                    AccountName = f.Message.Name,
                    f.Message.Created
                })));
        }
    }

    public class ChangedAccountNameEventHandlerTestsFixture
    {
        public TestableMessageHandlerContext MessageHandlerContext { get; set; }
        public List<long> Ukprns { get; set; }
        public ChangedAccountNameEvent Message { get; set; }
        public IHandleMessages<ChangedAccountNameEvent> Handler { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public GetAccountProviderUkprnsQueryResult GetAccountProviderUkprnsQueryResult { get; set; }
        
        public ChangedAccountNameEventHandlerTestsFixture()
        {
            MessageHandlerContext = new TestableMessageHandlerContext();
            
            Ukprns = new List<long>
            {
                11111111,
                22222222,
                33333333
            };
            
            Message = new ChangedAccountNameEvent(1, "Foo", DateTime.UtcNow);
            Mediator = new Mock<IMediator>();
            GetAccountProviderUkprnsQueryResult = new GetAccountProviderUkprnsQueryResult(Ukprns);

            Mediator.Setup(m => m.Send(It.Is<GetAccountProviderUkprnsQuery>(q => q.AccountId == Message.AccountId), CancellationToken.None)).ReturnsAsync(GetAccountProviderUkprnsQueryResult);
            
            Handler = new ChangedAccountNameEventHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Message, MessageHandlerContext);
        }
    }
}