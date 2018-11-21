using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.ReadStore.EventHandlers
{
    internal class AccountProviderLegalEntityUdatedPermissionsEventHandler : IHandleMessages<AccountProviderLegalEntityUdatedPermissionsEvent>
    {
        private readonly IReadStoreMediator _mediator;

        public AccountProviderLegalEntityUdatedPermissionsEventHandler(IReadStoreMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(AccountProviderLegalEntityUdatedPermissionsEvent message, IMessageHandlerContext context)
        {
            return _mediator.Send(new UpdateRelationshipCommand(message.Ukprn, message.AccountProviderId, message.AccountId, message.AccountLegalEntityId, 
                message.Operations, context.MessageId, message.Created));
        }
    }
}