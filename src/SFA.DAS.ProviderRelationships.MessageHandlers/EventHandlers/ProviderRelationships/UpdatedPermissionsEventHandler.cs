using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using UpdatePermissionsCommand = SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.UpdatePermissionsCommand;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships
{
    public class UpdatedPermissionsEventHandler : IHandleMessages<UpdatedPermissionsEvent>
    {
        private readonly IReadStoreMediator _readStoreMediator;
        private readonly IMediator _mediator;

        public UpdatedPermissionsEventHandler(IReadStoreMediator readStoreMediator, IMediator mediator)
        {
            _readStoreMediator = readStoreMediator;
            _mediator = mediator;
        }

        public Task Handle(UpdatedPermissionsEvent message, IMessageHandlerContext context)
        {
            return Task.WhenAll(
                _mediator.Send(new UpdatedPermissionsEventAuditCommand(message.AccountId,
                    message.AccountLegalEntityId,
                    message.AccountProviderId, message.AccountProviderLegalEntityId, message.Ukprn, message.UserRef,
                    message.GrantedOperations, message.Updated)),

                _readStoreMediator.Send(new UpdatePermissionsCommand(
                message.AccountId,
                message.AccountLegalEntityId,
                message.AccountProviderId,
                message.AccountProviderLegalEntityId,
                message.Ukprn,
                message.GrantedOperations,
                message.Updated,
                context.MessageId)));
        }
    }
}