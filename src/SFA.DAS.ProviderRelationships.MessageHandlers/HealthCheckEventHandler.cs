using System;
using System.Data.Entity;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Messages;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class HealthCheckEventHandler : ProviderRelationshipsEventHandler, IHandleMessages<HealthCheckEvent>
    {
        public HealthCheckEventHandler(Lazy<ProviderRelationshipsDbContext> db)
            : base(db, null)
        {
        }

        public async Task Handle(HealthCheckEvent message, IMessageHandlerContext context)
        {
            var healthCheck = await Db.HealthChecks.SingleAsync(h => h.Id == message.Id);

            healthCheck.ReceiveProviderRelationshipsEvent(message);
        }
    }
}