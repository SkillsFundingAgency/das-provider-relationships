using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.ReadStore.EventHandlers
{
    internal class AccountProviderLegalEntityCreatedEventHandler : IHandleMessages<AccountProviderLegalEntityCreatedEvent>
    {
        private readonly IRelationshipsRepository _relationshipsRepository;

        public AccountProviderLegalEntityCreatedEventHandler(IRelationshipsRepository relationshipsRepository)
        {
            _relationshipsRepository = relationshipsRepository;
        }

        public async Task Handle(AccountProviderLegalEntityCreatedEvent message, IMessageHandlerContext context)
        {
            var permission = await _relationshipsRepository.CreateQuery().SingleOrDefaultAsync(p => 
                        p.Provider.Ukprn == message.Ukprn &&
                        p.AccountProvider.Id == message.AccountProviderId && 
                        p.AccountLegalEntity.Id == message.AccountLegalEntityId);

            if (permission == null)
            {
                permission = new Relationship(message.Ukprn, message.AccountId, message.AccountPublicHashedId, message.AccountName,
                    message.AccountLegalEntityId, message.AccountLegalEntityPublicHashedId,
                    message.AccountLegalEntityName,
                    message.AccountProviderId, message.Created, context.MessageId);
                await _relationshipsRepository.Add(permission);
            }
            else
            {
                permission.Recreate(message.Ukprn, message.AccountId, message.AccountPublicHashedId, message.AccountName,
                    message.AccountLegalEntityId, message.AccountLegalEntityPublicHashedId,
                    message.AccountLegalEntityName,
                    message.AccountProviderId, message.Created, context.MessageId);
                await _relationshipsRepository.Update(permission);
            }
        }
    }
}