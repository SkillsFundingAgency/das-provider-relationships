using System;
using Newtonsoft.Json;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    public class OutboxDataItem
    {
        [JsonProperty("messageId")]
        public string MessageId { get; protected set; }

        [JsonProperty("created")]
        public DateTime Created { get; protected set; }

        public OutboxDataItem(string messageId, DateTime created)
        {
            MessageId = messageId;
            Created = created;
        }


    }
}
