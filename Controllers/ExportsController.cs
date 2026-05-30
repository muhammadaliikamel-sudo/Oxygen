using Microsoft.AspNetCore.Mvc;
using Oxygen.Interfaces;

namespace Oxygen.Controllers
{
    [ApiController]
    [Route("exports")]
    public class ExportsController : ControllerBase
    {
        private readonly IExportService _exportService;

        public ExportsController(IExportService exportService)
        {
            _exportService = exportService;
        }

        [HttpGet("visitors")]
        public async Task<IActionResult> ExportVisitors(
            [FromQuery] string? format,
            [FromQuery] Guid? userId,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _exportService.ExportVisitorsAsync(format ?? "csv", userId, cancellationToken);
                return File(result.Content, result.ContentType, result.FileName);
            }
            catch (NotSupportedException ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, ex.Message);
            }
        }

        [HttpGet("ctas")]
        public async Task<IActionResult> ExportCtas(
            [FromQuery] string? format,
            [FromQuery] Guid? userId,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _exportService.ExportCtasAsync(format ?? "csv", userId, cancellationToken);
                return File(result.Content, result.ContentType, result.FileName);
            }
            catch (NotSupportedException ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, ex.Message);
            }
        }

        [HttpGet("final")]
        public async Task<IActionResult> ExportFinal(
            [FromQuery] Guid? userId,
            CancellationToken cancellationToken)
        {
            var result = await _exportService.ExportFinalAsync(userId, cancellationToken);
            return File(result.Content, result.ContentType, result.FileName);
        }
    }
}
