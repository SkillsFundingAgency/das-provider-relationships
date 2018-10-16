using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.NLog.Logger;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.EmployerAccounts.Messages.Events
{
    public class UpdatedLegalEntityEvent : Event
    {
        public long AccountLegalEntityId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
    }
}

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class UpdatedLegalEntityEventHandler : ProviderRelationshipsEventHandler, IHandleMessages<UpdatedLegalEntityEvent>
    {
        public UpdatedLegalEntityEventHandler(Lazy<IProviderRelationshipsDbContext> db, ILog log)
            : base(db, log)
        {
        }

        public async Task Handle(UpdatedLegalEntityEvent message, IMessageHandlerContext context)
        {
            Log.Info($"Received: {message.AccountLegalEntityId}, '{message.Name}', User:{message.UserRef}");

            var accountLegalEntity = Db.AccountLegalEntities.Single(ale => ale.Id == message.AccountLegalEntityId);

            accountLegalEntity.Name = message.Name;

            await Db.SaveChangesAsync();
        }
    }
}
