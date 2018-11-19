using System;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    public class BatchDeleteRelationshipsCommand : IReadStoreRequest<Unit>
    {
        public long Ukprn { get; }
        public long AccountLegalEntityId { get; }
        public DateTime Created { get; }

        public BatchDeleteRelationshipsCommand(long ukprn, long accountLegalEntityId, DateTime created)
        {
            Ukprn = ukprn;
            AccountLegalEntityId = accountLegalEntityId;
            Created = created;
        }
    }
}