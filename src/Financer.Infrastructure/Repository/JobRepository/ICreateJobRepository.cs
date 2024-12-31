using System;
using System.Threading.Tasks;
using Financer.DataAccess.Entities.Jobs;

namespace Financer.Infrastructure.Repository.JobRepository
{
    public interface ICreateJobRepository
    {
        Task<Job> CreateJobAsync(CreateJob job);
    }
}
