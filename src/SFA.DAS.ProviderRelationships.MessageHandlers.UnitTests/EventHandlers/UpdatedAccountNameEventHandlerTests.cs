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
    public class UpdatedAccountNameEventHandlerTests : FluentTest<UpdatedAccountNameEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingUpdatedAccountNameEvent_ThenShouldSendBatchUpdateRelationshipAccountNamesCommands()
        {
            return RunAsync(f => f.Handle(), f => f.MessageHandlerContext.SentMessages
                .Select(m => m.Message)
                .Cast<BatchUpdateRelationshipAccountNamesCommand>()
                .Should()
                .BeEquivalentTo(f.Ukprns.Select(u => new
                {
                    Ukprn = u,
                    f.Message.AccountId,
                    f.Message.Name,
                    f.Message.Created
                })));
        }
    }

    public class UpdatedAccountNameEventHandlerTestsFixture
    {
        public TestableMessageHandlerContext MessageHandlerContext { get; set; }
        public List<long> Ukprns { get; set; }
        public UpdatedAccountNameEvent Message { get; set; }
        public IHandleMessages<UpdatedAccountNameEvent> Handler { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public GetAccountProviderUkprnsByAccountIdQueryResult GetAccountProviderUkprnsByAccountIdQueryResult { get; set; }
        
        public UpdatedAccountNameEventHandlerTestsFixture()
        {
            MessageHandlerContext = new TestableMessageHandlerContext();
            
            Ukprns = new List<long>
            {
                11111111,
                22222222,
                33333333
            };
            
            Message = new UpdatedAccountNameEvent(1, "Foo", DateTime.UtcNow);
            Mediator = new Mock<IMediator>();
            GetAccountProviderUkprnsByAccountIdQueryResult = new GetAccountProviderUkprnsByAccountIdQueryResult(Ukprns);

            Mediator.Setup(m => m.Send(It.Is<GetAccountProviderUkprnsByAccountIdQuery>(q => q.AccountId == Message.AccountId), CancellationToken.None)).ReturnsAsync(GetAccountProviderUkprnsByAccountIdQueryResult);
            
            Handler = new UpdatedAccountNameEventHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Message, MessageHandlerContext);
        }
    }
}