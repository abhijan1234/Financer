using System;
using System.Threading.Tasks;
using Financer.DataAccess.Entities.Jobs;

namespace Financer.Infrastructure.Services.JobServices
{
    public interface ICreateJobService
    {
        Task<Job> CreateJobAsync(CreateJob job);
    }
}
