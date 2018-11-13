using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class Relationship : Document
    {
        [JsonProperty("ukprn")]
        public long Ukprn { get; protected set; }

        [JsonProperty("accountProviderLegalEntityId")]
        public long AccountProviderLegalEntityId { get; protected set; }

        [JsonProperty("accountId")]
        public long AccountId { get; protected set; }

        [JsonProperty("accountPublicHashedId")]
        public string AccountPublicHashedId { get; protected set; }

        [JsonProperty("accountName")]
        public string AccountName { get; protected set; }


        [JsonProperty("accountLegalEntityId")]
        public long AccountLegalEntityId { get; protected set; }

        [JsonProperty("accountLegalEntityPublicHashedId")]
        public string AccountLegalEntityPublicHashedId { get; protected set; }

        [JsonProperty("accountLegalEntityName")]
        public string AccountLegalEntityName { get; protected set; }


        [JsonProperty("accountProviderId")]
        public int AccountProviderId { get; protected set; }

        [JsonProperty("operations")]
        public IEnumerable<Operation> Operations { get; protected set; }

        [JsonProperty("outboxData")]
        public IEnumerable<OutboxMessage> OutboxData {
            get => _outboxData.AsEnumerable();
            protected set => _outboxData = value.ToList();
        }

        [JsonProperty("created")]
        public DateTime Created { get; protected set; }

        [JsonProperty("deleted")]
        public DateTime? Deleted { get; protected set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; protected set; }

        [JsonIgnore]
        private List<OutboxMessage> _outboxData = new List<OutboxMessage>();

        public Relationship(long ukprn, long accountProviderLegalEntityId,
            long accountId, string accountPublicHashedId, string accountName, 
            long accountLegalEntityId, string accountLegalEntityPublicHashedId, string accountLegalEntityName, 
            int accountProviderId, DateTime created, string messageId)
            : base(1, "permission")
        {
            Ukprn = ukprn;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;

            AccountId = accountId;
            AccountPublicHashedId = accountPublicHashedId;
            AccountName = accountName;

            AccountLegalEntityId = accountLegalEntityId;
            AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
            AccountLegalEntityName = accountLegalEntityName;

            AccountProviderId = accountProviderId;
            Created = created;

            AddMessageToOutbox(messageId, created);
        }

        [JsonConstructor]
        protected Relationship()
        {
        }

        public void Recreate(long ukprn, long accountProviderLegalEntityId, 
            long accountId, string accountPublicHashedId, string accountName, 
            long accountLegalEntityId, string accountLegalEntityPublicHashedId, string accountLegalEntityName, 
            int accountProviderId, DateTime reactivated, string messageId)
        {
            ProcessMessage(messageId, reactivated, () =>
            {
                EnsureRelationshipIsDeleted();
                Ukprn = ukprn;
                AccountProviderLegalEntityId = accountProviderLegalEntityId;

                AccountId = accountId;
                AccountPublicHashedId = accountPublicHashedId;
                AccountName = accountName;

                AccountLegalEntityId = accountLegalEntityId;
                AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
                AccountLegalEntityName = accountLegalEntityName;

                AccountProviderId = accountProviderId;
                Created = reactivated;
                Deleted = null;
            });
        }

        public void UpdatePermissions(HashSet<Operation> grants, DateTime updated, string messageId)
        {
            ProcessMessage(messageId, updated,
                () =>
                {
                    EnsureRelationshipIsNotDeleted();
                    Operations = grants;
                    Updated = updated;
                }
            );
        }

        public void DeleteRelationship(DateTime deleted, string messageId)
        {
            ProcessMessage(messageId, deleted, () =>
            {
                EnsureRelationshipIsNotDeleted();
                Operations = new HashSet<Operation>();
                Deleted = deleted;
            });
        }

        private void ProcessMessage(string messageId, DateTime messageCreated, Action action)
        {
            if (MessageAlreadyProcessed(messageId))
                return;

            AddMessageToOutbox(messageId, messageCreated);
            if (!IsMessageChronological(messageCreated))
            {
                return;
            }
            action();
        }

        private bool IsMessageChronological(DateTime messageDateTime)
        {
            var updated = Updated ?? DateTime.MinValue;
            var deleted = Deleted ?? DateTime.MinValue;

            return messageDateTime > Created && messageDateTime >  updated && messageDateTime > deleted;
        }

        private void EnsureRelationshipIsNotDeleted()
        {
            if (Deleted != null)
                throw new InvalidOperationException("Relationship has been deleted");
        }

        private void EnsureRelationshipIsDeleted()
        {
            if (Deleted == null)
                throw new InvalidOperationException("Relationship has not been deleted");
        }

        private bool MessageAlreadyProcessed(string messageId)
        {
            return OutboxData.Any(x => x.MessageId == messageId);
        }

        private void AddMessageToOutbox(string messageId, DateTime created)
        {
            if (messageId is null) throw new ArgumentNullException(nameof(messageId));
            _outboxData.Add(new OutboxMessage(messageId, created));
        }
    }
}