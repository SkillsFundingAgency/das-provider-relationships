using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class UpdatedLegalEntityEvent : Event
    {
        public long AccountLegalEntityId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
    }

    public class UpdatedLegalEntityEventHandler : ProviderRelationshipsEventHandler, IHandleMessages<UpdatedLegalEntityEvent>
    {
        public UpdatedLegalEntityEventHandler(Lazy<ProviderRelationshipsDbContext> db)
            : base(db)
        {
        }

        public async Task Handle(UpdatedLegalEntityEvent message, IMessageHandlerContext context)
        {
            //todo: log inc. username/ref

            //todo: if we go with AddOrUpdate, what happens if we receive the update first, then the add -> will end up with incorrect version!

            //Db.Value.AccountLegalEntities.AddOrUpdate(new AccountLegalEntity
            //{
            //    AccountLegalEntityId = message.AccountLegalEntityId,
            //    Name = message.Name,
            //    //todo: add to message? rename?
            //    PublicHashedId = "123456", //todo: message.AccountLegalEntityPublicHashedId, or not store locally?
            //    AccountId = message.AccountId //todo: add accountid, so we can add or update?
            //});

            //todo: unit tests to check idempotency

            var accountLegalEntity = Db.Value.AccountLegalEntities.Single(ale => ale.AccountLegalEntityId == message.AccountLegalEntityId);

            accountLegalEntity.Name = message.Name;

            await Db.Value.SaveChangesAsync();
        }
    }
}
