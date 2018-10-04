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

    public class CreatedAccountEventHandler : IHandleMessages<CreatedAccountEvent>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public CreatedAccountEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task Handle(CreatedAccountEvent message, IMessageHandlerContext context)
        {
            //todo: log inc. username/ref

            _db.Value.Accounts.AddOrUpdate(new Account
            {
                AccountId = message.AccountId,
                Name = message.Name,
                PublicHashedId = message.PublicHashedId
            });

            await _db.Value.SaveChangesAsync();
        }
    }
}
