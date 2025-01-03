using System;
using Financer.DataAccess.Entities.Jobs;
using MongoDB.Bson.Serialization.Attributes;

namespace Financer.DataAccess.Entities.Outbox
{
    [BsonIgnoreExtraElements]
    public class OutboxEvent
    {
        [BsonElement("EventId")]
        public string EventId { get; set; }

        [BsonElement("Content")]
        public Job? Content { get; set; }

        [BsonElement("EventCreateTime")]
        public DateTime EventCreateTime { get; set; } = DateTime.UtcNow;

        [BsonElement("EventProcessTime")]
        public DateTime? EventProcessTime { get; set; }

        [BsonElement("Error")]
        public string? Error { get; set; }

        [BsonElement("IsSent")]
        public bool IsSent { get; set; }
    }
}
