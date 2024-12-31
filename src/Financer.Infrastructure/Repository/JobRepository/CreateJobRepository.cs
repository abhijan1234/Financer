using System;
using System.Threading.Tasks;
using Financer.DataAccess;
using Financer.DataAccess.Entities.Jobs;
using Financer.DataAccess.Entities.Outbox;
using Financer.DataAccess.Services.DatabaseService;
using MongoDB.Driver;

namespace Financer.Infrastructure.Repository.JobRepository
{
    public class CreateJobRepository : ICreateJobRepository
    {
        private readonly IMongoService _mongoService;
        private readonly IMongoDatabase _database;


        public CreateJobRepository(IMongoService mongoService, IMongoDatabase mongoDatabase)
        {
            _mongoService = mongoService;
            _database = mongoDatabase;
        }

        public async Task<Job> CreateJobAsync(CreateJob job)
        {
            Job newJob = new Job
            {
                JobType = job.JobType,
                JobId = Guid.NewGuid().ToString(),
                JobStatus = "Started",
                LastUpdated = DateTime.UtcNow,
                UserId = Guid.NewGuid().ToString()
            };

            OutboxEvent outbox = new OutboxEvent
            {
                EventName = Constants.EventInfo.JobCreateEvent,
                Content = newJob,
                InProcessed = false
            };

            using var session = await _database.Client.StartSessionAsync();

            try
            {
                session.StartTransaction();
                await _mongoService.InsertAync(newJob, Constants.MongoInfo.JobCollection, session);
                await _mongoService.InsertAync(outbox, Constants.MongoInfo.OutboxCollection, session);
                await session.CommitTransactionAsync();
                return newJob;
            }
            catch (Exception ex)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }
    }
}
