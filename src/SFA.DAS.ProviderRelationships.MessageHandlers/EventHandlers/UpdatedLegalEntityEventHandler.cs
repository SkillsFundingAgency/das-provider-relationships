using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
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

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    public class UpdatedLegalEntityEventHandler : IHandleMessages<UpdatedLegalEntityEvent>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public UpdatedLegalEntityEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task Handle(UpdatedLegalEntityEvent message, IMessageHandlerContext context)
        {
            var accountLegalEntity = await _db.Value.AccountLegalEntities.SingleAsync(ale => ale.Id == message.AccountLegalEntityId);

            accountLegalEntity.Name = message.Name;
        }
    }
}