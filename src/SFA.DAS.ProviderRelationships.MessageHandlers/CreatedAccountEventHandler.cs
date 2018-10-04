using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Messages;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class CreatedAccountEvent : Event
    {
        public long AccountId { get; set; }
        public string PublicHashedId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
    }

    public class CreatedAccountEventHandler : ProviderRelationshipsEventHandler, IHandleMessages<CreatedAccountEvent>
    {
        public CreatedAccountEventHandler(Lazy<ProviderRelationshipsDbContext> db)
            : base(db)
        {
        }

        public async Task Handle(CreatedAccountEvent message, IMessageHandlerContext context)
        {
            //todo: log inc. username/ref

            Db.Value.Accounts.AddOrUpdate(new Account
            {
                AccountId = message.AccountId,
                Name = message.Name,
                //todo: rename in event to AccountPublicHashedId, so that it's explicitly named and not confused with other ids?
                PublicHashedId = message.PublicHashedId
            });

            await Db.Value.SaveChangesAsync();
        }
    }
}
