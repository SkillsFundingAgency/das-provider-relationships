using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Data;

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
            var accountLegalEntity = await _db.Value.AccountLegalEntities.SingleAsync(a => a.Id == message.AccountLegalEntityId);

            accountLegalEntity.ChangeName(message.Name, message.Created);
        }
    }
}