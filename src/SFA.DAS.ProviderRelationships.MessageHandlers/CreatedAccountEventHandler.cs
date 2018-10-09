using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.NLog.Logger;
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
    public class CreatedAccountEventHandler : ProviderRelationshipsEventHandler, IHandleMessages<CreatedAccountEvent>
    {
        public CreatedAccountEventHandler(Lazy<IProviderRelationshipsDbContext> db, ILog log)
            : base(db, log)
        {
        }

        public async Task Handle(CreatedAccountEvent message, IMessageHandlerContext context)
        {
            Log.Info($"Received: {message.AccountId} ({message.PublicHashedId}), '{message.Name}', User:{message.UserRef}");

            Db.Accounts.Add(new Account
            {
                AccountId = message.AccountId,
                Name = message.Name,
                //todo: rename in event to AccountPublicHashedId, so that it's explicitly named and not confused with other ids?
                PublicHashedId = message.PublicHashedId
            });

            await Db.SaveChangesAsync();
        }
    }
}
