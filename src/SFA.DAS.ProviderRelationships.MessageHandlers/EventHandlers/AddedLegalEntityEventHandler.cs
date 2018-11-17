using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    public class AddedLegalEntityEventHandler : IHandleMessages<AddedLegalEntityEvent>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public AddedLegalEntityEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public Task Handle(AddedLegalEntityEvent message, IMessageHandlerContext context)
        {
            var accountLegalEntity = new AccountLegalEntity(
                message.AccountLegalEntityId,
                message.AccountLegalEntityPublicHashedId,
                message.AccountId,
                message.OrganisationName,
                message.Created);
            
            _db.Value.AccountLegalEntities.Add(accountLegalEntity);

            return Task.CompletedTask;
        }
    }
}