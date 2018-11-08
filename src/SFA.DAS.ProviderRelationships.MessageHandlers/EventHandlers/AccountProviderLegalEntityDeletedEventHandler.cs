using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    internal class AccountProviderLegalEntityDeletedEventHandler : IHandleMessages<AccountProviderLegalEntityDeletedEvent>
    {
        private readonly IPermissionsRepository _permissionsRepository;

        public AccountProviderLegalEntityDeletedEventHandler(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public async Task Handle(AccountProviderLegalEntityDeletedEvent message, IMessageHandlerContext context)
        {
            var permission = await _permissionsRepository.CreateQuery().FirstOrDefaultAsync(p => p.Ukprn == message.Ukprn && p.AccountProviderLegalEntityId == message.AccountProviderLegalEntityId);

            if (permission == null)
                throw new Exception("No Permission Found");

            permission.DeleteRelationship(message.Created, context.MessageId);
            await _permissionsRepository.Update(permission);
        }
    }
}