using System.Threading.Tasks;
using Financer.DataAccess.Entities.Jobs;
using Financer.Infrastructure.Repository.JobRepository;
using Microsoft.AspNetCore.Mvc;

namespace Financer.API.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class JobsController : Controller
    {
        private readonly ICreateJobRepository _createJobRepository;
        public JobsController(ICreateJobRepository createJobRepository)
        {
            _createJobRepository = createJobRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJob job)
        {
            var createdJob = await _createJobRepository.CreateJobAsync(job);
            return Ok(createdJob);
        }
    }
}
