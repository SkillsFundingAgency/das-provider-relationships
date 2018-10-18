using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    public class CreatedAccountEventHandler : IHandleMessages<CreatedAccountEvent>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public CreatedAccountEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public Task Handle(CreatedAccountEvent message, IMessageHandlerContext context)
        {
            _db.Value.Accounts.Add(new Account(message.AccountId, message.Name, message.Created));

            return Task.CompletedTask;
        }
    }
}