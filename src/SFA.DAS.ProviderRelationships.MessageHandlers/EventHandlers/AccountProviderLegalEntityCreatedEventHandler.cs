using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    internal class AccountProviderLegalEntityCreatedEventHandler : IHandleMessages<AccountProviderLegalEntityCreatedEvent>
    {
        private readonly IPermissionsRepository _permissionsRepository;

        public AccountProviderLegalEntityCreatedEventHandler(IPermissionsRepository permissionsRepository)
        {
            _permissionsRepository = permissionsRepository;
        }

        public async Task Handle(AccountProviderLegalEntityCreatedEvent message, IMessageHandlerContext context)
        {
            var permission = await _permissionsRepository.CreateQuery().FirstOrDefaultAsync(p => p.Ukprn == message.Ukprn && p.AccountProviderLegalEntityId == message.AccountProviderLegalEntityId);

            if (permission == null)
            {
                permission = Permission.Create(message.Ukprn, message.AccountProviderLegalEntityId,
                    message.AccountId, message.AccountPublicHashedId, message.AccountName,
                    message.AccountLegalEntityId, message.AccountLegalEntityPublicHashedId, message.AccountLegalEntityName,
                    message.AccountProviderId, message.Created, context.MessageId);
                await _permissionsRepository.Add(permission);
            }
            else
            {
                permission.ReActivateRelationship(message.Ukprn, message.AccountProviderLegalEntityId,
                    message.AccountId, message.AccountPublicHashedId, message.AccountName,
                    message.AccountLegalEntityId, message.AccountLegalEntityPublicHashedId, message.AccountLegalEntityName,
                    message.AccountProviderId, message.Created, context.MessageId);
                await _permissionsRepository.Update(permission);
            }

        }
    }
}