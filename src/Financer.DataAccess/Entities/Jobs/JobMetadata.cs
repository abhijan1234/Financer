using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Financer.DataAccess.Entities.Jobs
{
    public class JobMetadata
    {
        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("JobStatus")]
        public string JobStatus { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("LastUpdated")]
        public DateTime? LastUpdated { get; set; }
    }
}
