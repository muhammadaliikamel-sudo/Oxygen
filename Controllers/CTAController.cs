using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oxygen.DTOs.CTA;
using Oxygen.Interfaces;


namespace Oxygen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CTAController : ControllerBase
    {
        private readonly ICTAService _ctaService;
        public CTAController(ICTAService ctaService)
        {_ctaService = ctaService;}
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCTADto dto)
        {
            var result = await _ctaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {var result = await _ctaService.GetAllAsync();
         return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _ctaService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCTADto dto)
        {
            var result = await _ctaService.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _ctaService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
