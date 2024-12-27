using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Financer.DataAccess.Entities.Jobs
{
    public class Job : CreateJob
    {
        [BsonId]
        public string JobId { get; set; }

        [BsonElement("JobStatus")]
        public string JobStatus { get; set; }

        [BsonElement("LastUpdated")]
        public DateTime? LastUpdated { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }
    }
}
