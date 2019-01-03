using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests.EventHandlers
{
    #pragma warning disable CS0618
    public class EventHandlerTestsFixture<TEvent, TEventHandler> where TEvent : Event, new()
                                                                 where TEventHandler : IHandleMessages<TEvent>
    #pragma warning restore CS0618
    {
        public Mock<IMediator> Mediator { get; set; }
        public TEvent Message { get; set; }
        public IHandleMessages<TEvent> Handler { get; set; }
        
        public EventHandlerTestsFixture(Func<IMediator, IHandleMessages<TEvent>> constructHandler = null)
        {
            Mediator = new Mock<IMediator>();
            
            //todo: autofixture event??
            Message = new TEvent
            {
                Created = DateTime.UtcNow
            };
            
            Handler = constructHandler != null ? constructHandler(Mediator.Object) : ConstructHandler();
        }

        public virtual Task Handle()
        {
            return Handler.Handle(Message, null);
        }

        private TEventHandler ConstructHandler()
        {
            return (TEventHandler)Activator.CreateInstance(typeof(TEventHandler), Mediator.Object);
        }

        public void VerifySend<TCommand>(Func<TCommand,TEvent,bool> verifyCommand) where TCommand : IRequest
        {
            Mediator.Verify(m => m.Send(It.Is<TCommand>(c => verifyCommand(c,Message)), CancellationToken.None), Times.Once);
        }
    }
}