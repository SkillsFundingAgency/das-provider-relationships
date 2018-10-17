using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    /// <remarks>
    /// If we receive this event *before* the CreatedAccountEvent, this will fail, but should then work on a subsequent retry,
    /// as long as the last retry happens after the CreatedAccountEvent is successfully handled.
    /// With the exponential back-off potentially retrying messages 1/2 day after the first try, that gives a decent sized window.
    /// (Alternatively, we could remove the FK constraint and have eventual consistency.)
    /// </remarks>
    public class AddedLegalEntityEventHandler : IHandleMessages<AddedLegalEntityEvent>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public AddedLegalEntityEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        public Task Handle(AddedLegalEntityEvent message, IMessageHandlerContext context)
        {
            _db.Value.AccountLegalEntities.Add(new AccountLegalEntity(
                message.AccountLegalEntityId,
                message.AccountLegalEntityPublicHashedId,
                message.AccountId,
                message.OrganisationName,
                message.Created));

            return Task.CompletedTask;
        }
    }
}