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
    public class RemovedLegalEntityEvent : Event
    {
        public long AccountId { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
        public long AgreementId { get; set; }
        public bool AgreementSigned { get; set; }
        public long LegalEntityId { get; set; }
        public string OrganisationName { get; set; }
        public long AccountLegalEntityId { get; set; }
    }
}

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class RemovedLegalEntityEventHandler : ProviderRelationshipsEventHandler, IHandleMessages<RemovedLegalEntityEvent>
    {
        public RemovedLegalEntityEventHandler(Lazy<IProviderRelationshipsDbContext> db, ILog log)
            : base(db, log)
        {
        }

        public async Task Handle(RemovedLegalEntityEvent message, IMessageHandlerContext context)
        {
            Log.Info($"Received: {message.AccountLegalEntityId}, User:{message.UserRef}");

            //var accountLegalEntity = await Db.Value.AccountLegalEntities.SingleAsync();

            //accountLegalEntity.Permissions.Clear();
            ////todo: require Equals/GetHashCode?
            //Db.Value.AccountLegalEntities.Remove(accountLegalEntity);

            //todo: this fails if there are permissions referencing the ale entity
            // check cascade delete... https://stackoverflow.com/questions/17487577/entity-framework-ef-code-first-cascade-delete-for-one-to-zero-or-one-relations
            
            // either enable cascade delete on the db (but what if permissions are loaded into memory)
            // or load ale & permissions into memory and then delete and let ef cascade delete (potentially much slower - need to load ale + n permissions, then 1+ delete)
            //https://stackoverflow.com/questions/21314113/entity-framework-6-code-first-cascade-delete
            Db.Delete(new AccountLegalEntity { Id = message.AccountLegalEntityId });
            //Db.Entry(accountLegalEntity).State = EntityState.Deleted;

            await Db.SaveChangesAsync();
        }
    }
}
