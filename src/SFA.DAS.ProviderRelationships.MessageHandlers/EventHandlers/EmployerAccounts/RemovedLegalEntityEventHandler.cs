using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class RemovedLegalEntityEventHandler : IHandleMessages<RemovedLegalEntityEvent>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public RemovedLegalEntityEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task Handle(RemovedLegalEntityEvent message, IMessageHandlerContext context)
        {
            var account = await _db.Value.Accounts.SingleAsync(a => a.Id == message.AccountId);
            var accountLegalEntity = await _db.Value.AccountLegalEntities.SingleAsync(ale => ale.Id == message.AccountLegalEntityId);

            account.RemoveAccountLegalEntity(accountLegalEntity, message.Created);
        }
    }
}