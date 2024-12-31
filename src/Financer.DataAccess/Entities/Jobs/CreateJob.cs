using System;
using Financer.DataAccess.Entities.Finance;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Financer.DataAccess.Entities.Jobs
{
    public class CreateJob
    {
        [BsonElement("JobType")]
        public string JobType { get; set; }

        [BsonElement("FinanceData")]
        public FinanceData FinanceData { get; set; }
    }
}
