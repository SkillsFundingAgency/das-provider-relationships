using System;
using SFA.DAS.NServiceBus;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    internal class BatchUpdateRelationshipAccountNamesCommand : Command, IReadStoreRequest<Unit>
    {
        public long Ukprn { get; }
        public long AccountId { get; }
        public string AccountName { get; }
        public DateTime Created { get; }

        public BatchUpdateRelationshipAccountNamesCommand(long ukprn, long accountId, string accountName,
            DateTime created)
        {
            Ukprn = ukprn;
            AccountId = accountId;
            AccountName = accountName;
            Created = created;
        }
    }
}