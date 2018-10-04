using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.NLog.Logger;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class ChangedAccountNameEvent : Event
    {
        public long AccountId { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
        public string PreviousName { get; set; }
        public string CurrentName { get; set; }
    }

    public class ChangedAccountNameEventHandler : ProviderRelationshipsEventHandler, IHandleMessages<ChangedAccountNameEvent>
    {
        public ChangedAccountNameEventHandler(Lazy<ProviderRelationshipsDbContext> db, ILog log)
            : base(db, log)
        {
        }

        public async Task Handle(ChangedAccountNameEvent message, IMessageHandlerContext context)
        {
            //todo: check log contains ChangedAccountNameEventHandler. label fields??
            Log.Info($"Received: {message.AccountId}, '{message.PreviousName}' => '{message.CurrentName}', User:{message.UserRef}");

            //todo: what to do if not found?
            Db.Accounts.Single(a => a.AccountId == message.AccountId).Name = message.CurrentName;

            await Db.SaveChangesAsync();
        }
    }
}
