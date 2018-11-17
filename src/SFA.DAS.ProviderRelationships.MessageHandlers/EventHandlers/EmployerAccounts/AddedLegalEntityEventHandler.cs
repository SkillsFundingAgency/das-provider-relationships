using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class AddedLegalEntityEventHandler : IHandleMessages<AddedLegalEntityEvent>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public AddedLegalEntityEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task Handle(AddedLegalEntityEvent message, IMessageHandlerContext context)
        {
            var account = await _db.Value.Accounts.SingleAsync(a => a.Id == message.AccountId);
            
            account.AddAccountLegalEntity(message.AccountLegalEntityId, message.AccountLegalEntityPublicHashedId, message.OrganisationName, message.Created);
        }
    }
}