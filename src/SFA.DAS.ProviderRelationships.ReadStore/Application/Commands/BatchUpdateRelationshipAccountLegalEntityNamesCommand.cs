using System;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    internal class BatchUpdateRelationshipAccountLegalEntityNamesCommand : IReadStoreRequest<Unit>
    {
        public long Ukprn { get; }
        public long AccountLegalEntityId { get; }
        public string Name { get; }
        public DateTime Created { get; }

        public BatchUpdateRelationshipAccountLegalEntityNamesCommand(long ukprn, long accountLegalEntityId, string name, DateTime created)
        {
            Ukprn = ukprn;
            AccountLegalEntityId = accountLegalEntityId;
            Name = name;
            Created = created;
        }
    }
}