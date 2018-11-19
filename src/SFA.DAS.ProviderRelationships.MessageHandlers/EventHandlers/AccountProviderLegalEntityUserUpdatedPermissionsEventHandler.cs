using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    internal class AccountProviderLegalEntityUserUpdatedPermissionsEventHandler : IHandleMessages<AccountProviderLegalEntityUserUpdatedPermissionsEvent>
    {
        private readonly IRelationshipsRepository _relationshipsRepository;

        public AccountProviderLegalEntityUserUpdatedPermissionsEventHandler(IRelationshipsRepository relationshipsRepository)
        {
            _relationshipsRepository = relationshipsRepository;
        }

        public async Task Handle(AccountProviderLegalEntityUserUpdatedPermissionsEvent message, IMessageHandlerContext context)
        {
            var permission = await _relationshipsRepository.CreateQuery().SingleAsync(p => p.Provider.Ukprn == message.Ukprn && 
                              p.AccountProvider.Id == message.AccountProviderId &&
                              p.AccountLegalEntity.Id == message.AccountLegalEntityId);

            permission.UpdatePermissions(message.Operations, message.Created, context.MessageId);
            await _relationshipsRepository.Update(permission);
        }
    }
}