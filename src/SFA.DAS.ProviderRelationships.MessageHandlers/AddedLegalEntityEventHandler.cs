using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.NLog.Logger;
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
        public string AccountLegalEntityPublicHashedId { get; set; }
    }

    /// <remarks>
    /// If we receive this event *before* the CreatedAccountEvent, this will fail, but should then work on a subsequent retry,
    /// as long as the last retry happens after the CreatedAccountEvent is successfully handled.
    /// With the exponential back-off potentially retrying messages 1/2 day after the first try, that gives a decent sized window.
    /// (Alternatively, we could remove the FK constraint and have eventual consistency.)
    /// </remarks>
    public class AddedLegalEntityEventHandler : ProviderRelationshipsEventHandler, IHandleMessages<AddedLegalEntityEvent>
    {
        public AddedLegalEntityEventHandler(Lazy<IProviderRelationshipsDbContext> db, ILog log)
            : base(db, log)
        {
        }

        public async Task Handle(AddedLegalEntityEvent message, IMessageHandlerContext context)
        {
            Log.Info($"Received: {message.AccountLegalEntityId}, {message.AccountId}, {message.OrganisationName}, {message.UserRef}");

            Db.AccountLegalEntities.Add(new AccountLegalEntity
            {
                AccountLegalEntityId = message.AccountLegalEntityId,
                Name = message.OrganisationName,
                PublicHashedId = message.AccountLegalEntityPublicHashedId,
                AccountId = message.AccountId
            });

            await Db.SaveChangesAsync();
        }
    }
}
