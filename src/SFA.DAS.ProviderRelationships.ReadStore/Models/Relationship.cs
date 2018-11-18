using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class Relationship : Document
    {
        [JsonProperty("ap")]
        public AccountProvider AccountProvider { get; protected set; }

        [JsonProperty("aple")]
        public AccountProviderLegalEntity AccountProviderLegalEntity { get; protected set; }

        [JsonProperty("permissions")]
        public Permissions Permissions { get; protected set; }

        [JsonProperty("created")]
        public DateTime Created { get; protected set; }

        [JsonProperty("deleted")]
        public DateTime? Deleted { get; protected set; }


        [JsonProperty("outboxData")]
        public IEnumerable<OutboxMessage> OutboxData  => _outboxData;

        [JsonIgnore]
        private readonly List<OutboxMessage> _outboxData = new List<OutboxMessage>();

        public Relationship(long ukprn, long accountProviderLegalEntityId,
            long accountId, string accountPublicHashedId, string accountName, 
            long accountLegalEntityId, string accountLegalEntityPublicHashedId, string accountLegalEntityName, 
            int accountProviderId, DateTime created, string messageId)
            : base(1, "relationship")
        {
            AccountProvider = new AccountProvider(ukprn, accountId, accountPublicHashedId, accountName, accountProviderId);
            AccountProviderLegalEntity = new AccountProviderLegalEntity(accountProviderLegalEntityId, accountLegalEntityId, accountLegalEntityPublicHashedId, accountLegalEntityName);
            Permissions = new Permissions(new HashSet<Operation>());
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
                AccountProvider = new AccountProvider(ukprn, accountId, accountPublicHashedId, accountName, accountProviderId);
                AccountProviderLegalEntity = new AccountProviderLegalEntity(accountProviderLegalEntityId, accountLegalEntityId, accountLegalEntityPublicHashedId, accountLegalEntityName);
                Permissions = new Permissions(new HashSet<Operation>());
                Deleted = null;
                Created = reactivated;
            });
        }

        public void UpdatePermissions(HashSet<Operation> grants, DateTime updated, string messageId)
        {
            ProcessMessage(messageId, updated,
                () =>
                {
                    EnsureRelationshipIsNotDeleted();
                    Permissions.UpdateOperations(grants, updated);
                }
            );
        }

        public void DeleteRelationship(DateTime deleted, string messageId)
        {
            ProcessMessage(messageId, deleted, () =>
            {
                EnsureRelationshipIsNotDeleted();
                Permissions = new Permissions(new HashSet<Operation>());
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
            var updated = Permissions.Updated ?? DateTime.MinValue;
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