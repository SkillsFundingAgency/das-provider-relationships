using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
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
        public ChangedAccountNameEventHandler(Lazy<ProviderRelationshipsDbContext> db)
            : base(db)
        {
        }

        public async Task Handle(ChangedAccountNameEvent message, IMessageHandlerContext context)
        {
            //todo: log inc. username/ref

            //todo: add publichashedid so we can AddOrUpdate?
            //_db.Value.Accounts.AddOrUpdate(new Account
            //{
            //    AccountId = message.AccountId,
            //    Name = message.CurrentName,
            //    PublicHashedId = message.PublicHashedId
            //});

            //todo: what to do if not found? addorupdate?
            Db.Value.Accounts.Single(a => a.AccountId == message.AccountId).Name = message.CurrentName;

            await Db.Value.SaveChangesAsync();
        }
    }
}
