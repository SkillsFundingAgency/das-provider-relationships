using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    internal class AccountProviderLegalEntityUserUpdatedPermissionsEventHandler : IHandleMessages<AccountProviderLegalEntityUserUpdatedPermissionsEvent>
    {
        private readonly IPermissionsRepository _permissionsRepository;

        public AccountProviderLegalEntityUserUpdatedPermissionsEventHandler(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public async Task Handle(AccountProviderLegalEntityUserUpdatedPermissionsEvent message, IMessageHandlerContext context)
        {
            var permission = await _permissionsRepository.CreateQuery().SingleAsync(p => p.Ukprn == message.Ukprn && p.AccountProviderLegalEntityId == message.AccountProviderLegalEntityId);

            permission.UpdatePermissions(message.Operations, message.Created, context.MessageId);
            await _permissionsRepository.Update(permission);
        }
    }
}