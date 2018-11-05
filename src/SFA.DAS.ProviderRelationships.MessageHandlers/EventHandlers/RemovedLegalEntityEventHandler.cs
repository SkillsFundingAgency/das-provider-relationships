using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using Z.EntityFramework.Plus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    public class RemovedLegalEntityEventHandler : IHandleMessages<RemovedLegalEntityEvent>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public RemovedLegalEntityEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task Handle(RemovedLegalEntityEvent message, IMessageHandlerContext context)
        {
            await _db.Value.AccountLegalEntities.Where(ale => ale.Id == message.AccountLegalEntityId).DeleteAsync();
            
            //todo: need to publish revokedpermission events - can we get away with deleted le event? yes
            // delete relationships
        }
    }
}