using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class ChangedAccountNameEventHandler : IHandleMessages<ChangedAccountNameEvent>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public ChangedAccountNameEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task Handle(ChangedAccountNameEvent message, IMessageHandlerContext context)
        {
            var account = await _db.Value.Accounts.SingleAsync(a => a.Id == message.AccountId);

            account.ChangeName(message.CurrentName, message.Created);
        }
    }
}