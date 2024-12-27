using System.Threading.Tasks;
using Financer.DataAccess.Entities.Jobs;
using Financer.Infrastructure.Services.JobServices;
using Microsoft.AspNetCore.Mvc;

namespace Financer.API.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class JobsController : Controller
    {
        private readonly ICreateJobService _createJobService;
        public JobsController(ICreateJobService createJobService)
        {
            _createJobService = createJobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJob job)
        {
            var createdJob = await _createJobService.CreateJobAsync(job);
            return Ok(createdJob);
        }
    }
}
