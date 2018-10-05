using System;
using System.Data.Entity;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.NLog.Logger;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
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

            // check cascade delete... https://stackoverflow.com/questions/17487577/entity-framework-ef-code-first-cascade-delete-for-one-to-zero-or-one-relations
            Db.Delete(new AccountLegalEntity { AccountLegalEntityId = message.AccountLegalEntityId });
            //Db.Entry(accountLegalEntity).State = EntityState.Deleted;

            await Db.SaveChangesAsync();
        }
    }
}
