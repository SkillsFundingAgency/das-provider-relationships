using System;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    internal class BatchUpdateRelationshipAccountNamesCommand : IReadStoreRequest<Unit>
    {
        public long Ukprn { get; }
        public long AccountId { get; }
        public string Name { get; }
        public DateTime Created { get; }

        public BatchUpdateRelationshipAccountNamesCommand(long ukprn, long accountId, string name, DateTime created)
        {
            Ukprn = ukprn;
            AccountId = accountId;
            Name = name;
            Created = created;
        }
    }
}