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

        [JsonProperty("accountProviderId")]
        public long AccountProviderId { get; protected set; }

        [JsonProperty("accountId")]
        public long AccountId { get; protected set; }

        [JsonProperty("accountLegalEntityId")]
        public long AccountLegalEntityId { get; protected set; }

        [JsonProperty("operations")]
        public IEnumerable<Operation> Operations { get; protected set; } = new HashSet<Operation>();

        [JsonProperty("outboxData")]
        public IEnumerable<OutboxMessage> OutboxData  => _outboxData;

        [JsonProperty("updated")]
        public DateTime Updated { get; protected set; }

        [JsonProperty("deleted")]
        public DateTime? Deleted { get; protected set; }

        [JsonIgnore]
        private readonly List<OutboxMessage> _outboxData = new List<OutboxMessage>();

        public Relationship(long ukprn, long accountProviderId, long accountId, long accountLegalEntityId, 
            HashSet<Operation> operations, string messageId, DateTime created)
            : base(1, "relationship")
        {
            Ukprn = ukprn;
            AccountProviderId = accountProviderId;
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            Operations = operations;
            Updated = created;

            AddMessageToOutbox(messageId, created);

            Id = Guid.NewGuid();
        }

        [JsonConstructor]
        protected Relationship()
        {
        }

        public void UpdatePermissions(HashSet<Operation> grants, DateTime updated, string messageId)
        {
            ProcessMessage(messageId, updated,
                () =>
                {
                    Operations = grants;
                    Updated = updated;
                    Deleted = null;
                }
            );
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
            var deleted = Deleted ?? DateTime.MinValue;

            return messageDateTime > Updated && messageDateTime > deleted;
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