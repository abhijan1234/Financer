namespace Financer.DataAccess
{
    public static class Constants
    {
        public class MongoInfo
        {
            public const string JobCollection = "Jobs";
            public const string OutboxCollection = "Outbox";
        }

        public class EventInfo
        {
            public const string JobCreateEvent = "JobCreated";
        }

        public class JobStatus
        {
            public const string Created = "Created";
            public const string InProgress = "InProgress";
            public const string Completed = "Completed";
            public const string Failed = "Failed";

        }
    } 
}
