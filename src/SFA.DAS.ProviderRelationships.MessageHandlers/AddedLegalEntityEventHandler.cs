using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    //todo: these will come from SFA.DAS.EmployerAccounts.Messages nuget package once it's available
    public class AddedLegalEntityEvent : Event
    {
        public long AccountId { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
        public string OrganisationName { get; set; }
        public long AgreementId { get; set; }
        public long LegalEntityId { get; set; }
        public long AccountLegalEntityId { get; set; }
    }

    public class AddedLegalEntityEventHandler : ProviderRelationshipsEventHandler, IHandleMessages<AddedLegalEntityEvent>
    {
        public AddedLegalEntityEventHandler(Lazy<ProviderRelationshipsDbContext> db)
            : base(db)
        {
        }

        public async Task Handle(AddedLegalEntityEvent message, IMessageHandlerContext context)
        {
            //todo: log inc. username/ref

            //todo what if we process the AddedLegalEntityEvent *before* we receive the CreatedAccountEvent?
            // either:
            // let it fail, and should work on a subsequent retry (unless CreatedAccountEvent takes longer than the retry window)
            // remove the foreign key relationship - eventual consistency (but would EF be able to link entities together - looks like it can without an explicit FK)
            Db.Value.AccountLegalEntities.AddOrUpdate(new AccountLegalEntity
            {
                AccountLegalEntityId = message.AccountLegalEntityId,
                Name = message.OrganisationName,
                //todo: add to message? rename?
                PublicHashedId = "123456", //message.AccountLegalEntityPublicHashedId
                AccountId = message.AccountId
            });

            await Db.Value.SaveChangesAsync();
        }

    }
}
