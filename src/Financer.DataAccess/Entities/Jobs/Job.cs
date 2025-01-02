using Financer.DataAccess.Entities.Finance;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Financer.DataAccess.Entities.Jobs
{
    [BsonIgnoreExtraElements]
    public class Job : JobMetadata
    {
        [BsonElement("JobId")]
        public string JobId { get; set; }

        [BsonElement("JobInfo")]
        public CreateJob JobInfo { get; set; }
    }
}
