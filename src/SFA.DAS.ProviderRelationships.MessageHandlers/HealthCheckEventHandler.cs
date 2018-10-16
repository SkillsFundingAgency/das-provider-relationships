using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Messages;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class HealthCheckEventHandler : IHandleMessages<HealthCheckEvent>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public HealthCheckEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public async Task Handle(HealthCheckEvent message, IMessageHandlerContext context)
        {
            var healthCheck = await _db.Value.HealthChecks.SingleAsync(h => h.Id == message.Id);

            healthCheck.ReceiveProviderRelationshipsEvent(message);
        }
    }
}