using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oxygen.DTOs.Interaction;
using Oxygen.Interfaces;

namespace Oxygen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InteractionController : ControllerBase
    {
        private readonly IInteractionService _interactionService;
        public InteractionController(IInteractionService interactionService)
        {
            _interactionService = interactionService;
        }
        // POST:Create Interaction
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInteractionDto dto)
        {
            var result = await _interactionService.CreateAsync(dto);
            return Ok(result);
        }

        // GET: All Interactions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _interactionService.GetAllAsync();
            return Ok(result);
        }
        // GET:by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _interactionService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        // PUT:Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateInteractionDto dto)
        {
            var result = await _interactionService.UpdateAsync(id, dto);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        // DELETE:
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _interactionService.DeleteAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }            
    }

}
