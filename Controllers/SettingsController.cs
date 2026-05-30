using Microsoft.AspNetCore.Mvc;
using Oxygen.DTOs;
using Oxygen.Interfaces;

namespace Oxygen.Controllers
{
    [ApiController]
    [Route("settings")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SettingDto>>> GetAll(CancellationToken cancellationToken)
        {
            var settings = await _settingsService.GetAllAsync(cancellationToken);
            return Ok(settings);
        }

        [HttpPatch]
        public async Task<ActionResult<IReadOnlyList<SettingDto>>> Update(
            [FromBody] UpdateSettingsRequestDto request,
            CancellationToken cancellationToken)
        {
            if (request.Settings.Count == 0)
            {
                return BadRequest("settings are required");
            }

            var settings = await _settingsService.UpdateAsync(request, cancellationToken);
            return Ok(settings);
        }
    }
}
