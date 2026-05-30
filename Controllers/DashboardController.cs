using Microsoft.AspNetCore.Mvc;
using Oxygen.DTOs;
using Oxygen.Interfaces;

namespace Oxygen.Controllers
{
    [ApiController]
    [Route("dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("overview")]
        public async Task<ActionResult<IReadOnlyList<DashboardMetricDto>>> GetOverview(
            CancellationToken cancellationToken)
        {
            var response = await _dashboardService.GetOverviewAsync(cancellationToken);
            return Ok(response);
        }

        [HttpGet("cta")]
        public async Task<ActionResult<IReadOnlyList<DashboardMetricDto>>> GetCta(
            CancellationToken cancellationToken)
        {
            var response = await _dashboardService.GetCtaAsync(cancellationToken);
            return Ok(response);
        }

        [HttpGet("candidates")]
        public async Task<ActionResult<IReadOnlyList<DashboardMetricDto>>> GetCandidates(
            CancellationToken cancellationToken)
        {
            var response = await _dashboardService.GetCandidatesAsync(cancellationToken);
            return Ok(response);
        }

        [HttpGet("realtime")]
        public async Task<ActionResult<IReadOnlyList<DashboardRealtimeItemDto>>> GetRealtime(
            [FromQuery] int? limit,
            CancellationToken cancellationToken)
        {
            var response = await _dashboardService.GetRealtimeAsync(limit ?? 20, cancellationToken);
            return Ok(response);
        }

        [HttpGet("drilldown/{metric}")]
        public async Task<ActionResult<IReadOnlyList<DrilldownItemDto>>> GetDrilldown(
            [FromRoute] string metric,
            [FromQuery] int? top,
            CancellationToken cancellationToken)
        {
            var response = await _dashboardService.GetDrilldownAsync(metric, top ?? 10, cancellationToken);
            return Ok(response);
        }
    }
}
