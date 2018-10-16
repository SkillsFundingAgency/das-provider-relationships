using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.EmployerAccounts.Messages.Events
{
    public class ChangedAccountNameEvent : Event
    {
        public long AccountId { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
        public string PreviousName { get; set; }
        public string CurrentName { get; set; }
    }
}

namespace SFA.DAS.ProviderRelationships.MessageHandlers
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
            
            //account.Name = message.CurrentName;
        }
    }
}