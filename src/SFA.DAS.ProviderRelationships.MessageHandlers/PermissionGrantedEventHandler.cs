using System;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerCommitments.Messages.Events;
using SFA.DAS.NLog.Logger;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

//todo: move these events into EmployerCommitments
namespace SFA.DAS.EmployerCommitments.Messages.Events
{
    public class PermissionGrantedEvent : Event
    {
        public long AccountLegalEntityId { get; set; }
        public PermissionType Type { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
    }
}

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class PermissionGrantedEventHandler : ProviderRelationshipsEventHandler, IHandleMessages<PermissionGrantedEvent>
    {
        public PermissionGrantedEventHandler(Lazy<IProviderRelationshipsDbContext> db, ILog log)
            : base(db, log)
        {
        }

        public async Task Handle(PermissionGrantedEvent message, IMessageHandlerContext context)
        {
            Log.Info($"Received: {message.AccountLegalEntityId}, '{(int)message.Type}:{message.Type}', User:{message.UserRef}");

            Db.Permissions.AddOrUpdate(p => new { p.AccountLegalEntityId, p.Type }, new Permission
            {
                AccountLegalEntityId = message.AccountLegalEntityId,
                Type = message.Type
            });

            await Db.SaveChangesAsync();
        }
    }
}
