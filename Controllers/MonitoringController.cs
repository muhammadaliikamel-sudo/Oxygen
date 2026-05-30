using Microsoft.AspNetCore.Mvc;
using Oxygen.DTOs;
using Oxygen.Interfaces;

namespace Oxygen.Controllers
{
    [ApiController]
    [Route("monitoring")]
    public class MonitoringController : ControllerBase
    {
        private readonly IMonitoringService _monitoringService;

        public MonitoringController(IMonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
        }

        [HttpGet("health")]
        public IActionResult GetHealth()
        {
            return Ok(new { status = "ok", timestamp = DateTime.UtcNow });
        }

        [HttpGet("queues")]
        public async Task<ActionResult<IReadOnlyList<QueueStatusDto>>> GetQueues(
            CancellationToken cancellationToken)
        {
            var status = await _monitoringService.GetQueueStatusAsync(cancellationToken);
            return Ok(status);
        }

        [HttpPost("jobs/backup")]
        public async Task<IActionResult> TriggerBackup(CancellationToken cancellationToken)
        {
            await _monitoringService.TriggerBackupAsync(cancellationToken);
            return Accepted();
        }

        [HttpPost("jobs/cleanup")]
        public async Task<IActionResult> TriggerCleanup(CancellationToken cancellationToken)
        {
            await _monitoringService.TriggerCleanupAsync(cancellationToken);
            return Accepted();
        }

        [HttpGet("jobs")]
        public async Task<ActionResult<IReadOnlyList<BackgroundJobRunDto>>> GetJobRuns(
            [FromQuery] int? limit,
            CancellationToken cancellationToken)
        {
            var response = await _monitoringService.GetJobRunsAsync(limit ?? 25, cancellationToken);
            return Ok(response);
        }
    }
}
