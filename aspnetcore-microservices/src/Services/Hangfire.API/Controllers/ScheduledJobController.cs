using Hangfire.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.ScheduledJob;

namespace Hangfire.API.Controllers
{
    [ApiController]
    [Route("api/scheduled-job")]
    public class ScheduledJobController : ControllerBase
    {
        private readonly IBackgroundJobService _jobService;
        public ScheduledJobController(IBackgroundJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost("send-mail-reminder-checkout-order")]
        public IActionResult SenMailRemindeeCheckoutOrder([FromBody] ReminderCheckoutOrderDto requestDto)
        {
            var jobId = _jobService.AutoSendMail(requestDto);

            return Ok(jobId);
        }
        [HttpDelete("deleted/{id}")]
        public IActionResult Delete(string id)
        {
            var jobId = _jobService.scheduleJobServiceProperty.Delete(id);

            return Ok(jobId);
        }
    }
}
