using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using Z.EntityFramework.Plus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

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
            //var accountLegalEntity = await Db.Value.AccountLegalEntities.SingleAsync();

            //accountLegalEntity.Permissions.Clear();
            ////todo: require Equals/GetHashCode?
            //Db.Value.AccountLegalEntities.Remove(accountLegalEntity);

            //todo: this fails if there are permissions referencing the ale entity
            // check cascade delete... https://stackoverflow.com/questions/17487577/entity-framework-ef-code-first-cascade-delete-for-one-to-zero-or-one-relations
            
            // either enable cascade delete on the db (but what if permissions are loaded into memory)
            // or load ale & permissions into memory and then delete and let ef cascade delete (potentially much slower - need to load ale + n permissions, then 1+ delete)
            //https://stackoverflow.com/questions/21314113/entity-framework-6-code-first-cascade-delete
            //_db.Delete(new AccountLegalEntity { Id = message.AccountLegalEntityId });
            //Db.Entry(accountLegalEntity).State = EntityState.Deleted;

//            _db.Value.Remove(new AccountLegalEntity {Id = message.AccountLegalEntityId});
//            return Task.CompletedTask;

            await _db.Value.AccountLegalEntities.Where(ale => ale.Id == message.AccountLegalEntityId).DeleteAsync();
            
            //todo: need to publish revokedpermission events - can we get away with deleted le event? yes
            // delete relationships
        }
    }
}