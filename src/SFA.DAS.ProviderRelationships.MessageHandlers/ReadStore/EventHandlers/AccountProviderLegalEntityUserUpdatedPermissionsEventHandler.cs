using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.ReadStore.EventHandlers
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
            var permission = await _relationshipsRepository.CreateQuery().SingleAsync(p => p.Ukprn == message.Ukprn && p.AccountProviderLegalEntityId == message.AccountProviderLegalEntityId);

            permission.UpdatePermissions(message.Operations, message.Created, context.MessageId);
            await _relationshipsRepository.Update(permission);
        }
    }
}