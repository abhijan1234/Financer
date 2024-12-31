using System;
using Financer.DataAccess.Entities.Jobs;

namespace Financer.DataAccess.Entities.Outbox
{
    public class OutboxEvent
    {
        public string EventName { get; set; }
        public Job? Content { get; set; }
        public DateTime EventCreateTime { get; set; } = DateTime.UtcNow;
        public DateTime? EventProcessTime { get; set; }
        public string? Error { get; set; }
        public bool InProcessed { get; set; }
    }
}
