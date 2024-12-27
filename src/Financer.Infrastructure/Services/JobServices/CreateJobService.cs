using System;
using System.Threading.Tasks;
using Financer.DataAccess;
using Financer.DataAccess.Entities.Jobs;
using Financer.DataAccess.Services;

namespace Financer.Infrastructure.Services.JobServices
{
    public class CreateJobService : ICreateJobService
    {
        private readonly IMongoService _mongoService;

        public CreateJobService(IMongoService mongoService)
        {
            _mongoService = mongoService;
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

            await _mongoService.InsertAync(newJob, Constants.MongoInfo.JobCollection);
            return newJob;
        }
    }
}
