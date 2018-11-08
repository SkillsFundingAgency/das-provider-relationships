using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    internal class AccountProviderLegalEntityUpdatedEventHandler : IHandleMessages<AccountProviderLegalEntityUpdatedEvent>
    {
        private readonly IPermissionsRepository _permissionsRepository;

        public AccountProviderLegalEntityUpdatedEventHandler(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public async Task Handle(AccountProviderLegalEntityUpdatedEvent message, IMessageHandlerContext context)
        {
            var permission = await _permissionsRepository.CreateQuery().FirstOrDefaultAsync(p => p.Ukprn == message.Ukprn && p.AccountProviderLegalEntityId == message.AccountProviderLegalEntityId);

            if (permission == null)
                throw new Exception("No Permission Found");

            permission.UpdatePermissions(message.Operations, message.Created, context.MessageId);
            await _permissionsRepository.Update(permission);
        }
    }
}