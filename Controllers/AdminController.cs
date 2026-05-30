using Microsoft.AspNetCore.Mvc;
using Oxygen.DTOs;
using Oxygen.Interfaces;

namespace Oxygen.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly ILookupService _lookupService;

        public AdminController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [HttpGet("lookups")]
        public async Task<ActionResult<IReadOnlyList<LookupListDto>>> GetLookups(
            [FromQuery] string? type,
            CancellationToken cancellationToken)
        {
            var response = await _lookupService.GetAsync(type, cancellationToken);
            return Ok(response);
        }

        [HttpPost("lookups")]
        public async Task<ActionResult<LookupListDto>> CreateLookup(
            [FromBody] CreateLookupListDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await _lookupService.CreateAsync(request, cancellationToken);
                return Created($"/admin/lookups/{response.Id}", response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("lookups/{id:guid}")]
        public async Task<ActionResult<LookupListDto>> UpdateLookup(
            Guid id,
            [FromBody] CreateLookupListDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await _lookupService.UpdateAsync(id, request, cancellationToken);
                if (response is null)
                {
                    return NotFound();
                }

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("lookups/{id:guid}")]
        public async Task<IActionResult> DeleteLookup(Guid id, CancellationToken cancellationToken)
        {
            var removed = await _lookupService.DeleteAsync(id, cancellationToken);
            if (!removed)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
