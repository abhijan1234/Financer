using System;
using Financer.OutboxEventService.Models.Finance;
using MongoDB.Bson.Serialization.Attributes;

namespace Financer.OutboxEventService.Models.Jobs
{
    public class JobEvent
    {
        [BsonElement("JobId")]
        public string JobId { get; set; }

        [BsonElement("JobType")]
        public string JobType { get; set; }

        [BsonElement("FinanceData")]
        public FinanceData FinanceData { get; set; }
    }
}
