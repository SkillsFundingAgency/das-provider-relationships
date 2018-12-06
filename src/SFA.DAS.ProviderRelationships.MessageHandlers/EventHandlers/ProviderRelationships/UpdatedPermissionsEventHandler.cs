using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Audit.Commands;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

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
            _mediator.Send(new UpdatedPermissionsEventAuditCommand {
                UserRef = message.UserRef,
                GrantedOperations = message.GrantedOperations.ToList(),
                AccountId = message.AccountId,
                Updated = message.Updated,
                AccountLegalEntityId = message.AccountLegalEntityId,
                AccountProviderLegalEntityId = message.AccountProviderLegalEntityId,
                AccountProviderId = message.AccountProviderId,
                Ukprn = message.Ukprn
            });

            return _readStoreMediator.Send(new UpdatePermissionsCommand(
                message.AccountId,
                message.AccountLegalEntityId,
                message.AccountProviderId,
                message.AccountProviderLegalEntityId,
                message.Ukprn,
                message.GrantedOperations,
                message.Updated,
                context.MessageId));
        }
    }
}