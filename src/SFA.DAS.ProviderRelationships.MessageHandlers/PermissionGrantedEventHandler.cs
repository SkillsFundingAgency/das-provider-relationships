using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Messages;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class PermissionGrantedEventHandler : IHandleMessages<PermissionGrantedEvent>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public PermissionGrantedEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public Task Handle(PermissionGrantedEvent message, IMessageHandlerContext context)
        {
            /*_db.Value.Permissions.AddOrUpdate(p => new { p.AccountLegalEntityId, p.Type }, new Permission
            {
                AccountLegalEntityId = message.AccountLegalEntityId,
                Type = message.Type
            });*/

            return Task.CompletedTask;
        }
    }
}