using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.EmployerAccounts.Messages.Events
{
    public class CreatedAccountEvent : Event
    {
        public long AccountId { get; set; }
        public string PublicHashedId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
    }
}

namespace SFA.DAS.ProviderRelationships.MessageHandlers
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
            var account = new Account(message.AccountId, message.Name, message.Created);
            
            _db.Value.Accounts.Add(account);

            return Task.CompletedTask;
        }
    }
}