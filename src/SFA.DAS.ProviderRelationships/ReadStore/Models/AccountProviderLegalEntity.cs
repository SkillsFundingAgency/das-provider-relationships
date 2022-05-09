using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.ProviderRelationships.Types.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    public class AccountProviderLegalEntity : Document, IAccountProviderLegalEntityDto
    {
        [JsonProperty("accountId")]
        public virtual long AccountId { get; private set; }

        [JsonProperty("accountLegalEntityId")]
        public virtual long AccountLegalEntityId { get; private set; }

        [JsonProperty("accountProviderId")]
        public virtual long AccountProviderId { get; private set; }

        [JsonProperty("accountProviderLegalEntityId")]
        public virtual long AccountProviderLegalEntityId { get; private set; }

        [JsonProperty("ukprn")]
        public virtual long Ukprn { get; private set; }

        [JsonProperty("operations")]
        public virtual IEnumerable<Operation> Operations { get; private set; } = new HashSet<Operation>();

        [JsonProperty("outboxData")]
        public virtual IEnumerable<OutboxMessage> OutboxData => _outboxData;

        [JsonProperty("created")]
        public virtual DateTime Created { get; private set; }

        [JsonProperty("updated")]
        public virtual DateTime? Updated { get; private set; }

        [JsonProperty("deleted")]
        public virtual DateTime? Deleted { get; private set; }

        [JsonIgnore]
        private readonly List<OutboxMessage> _outboxData = new List<OutboxMessage>();

        [JsonConstructor]
        public AccountProviderLegalEntity()
        {
        }

        public AccountProviderLegalEntity(long accountId, long accountLegalEntityId, long accountProviderId, long accountProviderLegalEntityId, long ukprn, HashSet<Operation> grantedOperations, DateTime created, string messageId)
            : base(1, "relationship")
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountProviderId = accountProviderId;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            Operations = grantedOperations;
            Created = created;

            AddOutboxMessage(messageId, created);
        }

        public void Delete(DateTime deleted, string messageId)
        {
            ProcessMessage(messageId, deleted, () =>
            {
                EnsureRelationshipHasNotBeenDeleted();

                Operations = new HashSet<Operation>();
                Deleted = deleted;
            });
        }

        public void UpdatePermissions(HashSet<Operation> grantedOperations, DateTime updated, string messageId)
        {
            ProcessMessage(messageId, updated, () =>
            {
                if (IsUpdatedPermissionsDateChronological(updated))
                {
                    EnsureRelationshipHasNotBeenDeleted();
                    
                    Operations = grantedOperations;
                    Updated = updated;
                }
            });
        }

        private void AddOutboxMessage(string messageId, DateTime created)
        {
            if (messageId is null)
            {
                throw new ArgumentNullException(nameof(messageId));
            }
            
            _outboxData.Add(new OutboxMessage(messageId, created));
        }

        private void EnsureRelationshipHasNotBeenDeleted()
        {
            if (Deleted != null)
            {
                throw new InvalidOperationException("Requires relationship has not been deleted");
            }
        }

        private bool IsMessageProcessed(string messageId)
        {
            return OutboxData.Any(m => m.MessageId == messageId);
        }

        private bool IsUpdatedPermissionsDateChronological(DateTime updated)
        {
            return updated > Created && (Updated == null || updated > Updated.Value) && (Deleted == null || updated > Deleted.Value);
        }

        private void ProcessMessage(string messageId, DateTime created, Action action)
        {
            if (!IsMessageProcessed(messageId))
            {
                action();
                AddOutboxMessage(messageId, created);
            }
        }
    }
}