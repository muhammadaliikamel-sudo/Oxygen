using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oxygen.DTOs;
using Oxygen.DTOs.Visitor;
using Oxygen.Interfaces;

namespace Oxygen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VisitorController : ControllerBase
    {
        private readonly IVisitorService _visitorService;

        public VisitorController(
            IVisitorService visitorService)
        {
            _visitorService = visitorService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateVisitorDto dto)
        {
            var result =
                await _visitorService.CreateAsync(dto);

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result =
                await _visitorService.GetAllAsync();

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            Guid id)
        {
            var result =
                await _visitorService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(
                    new
                    {
                        message = "Visitor not found"
                    });
            }

            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdateVisitorDto dto)
        {
            var result =
                await _visitorService.UpdateAsync(
                    id,
                    dto);

            if (result == null)
            {
                return NotFound(
                    new
                    {
                        message = "Visitor not found"
                    });
            }

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            Guid id)
        {
            var result =
                await _visitorService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(
                    new
                    {
                        message = "Visitor not found"
                    });
            }

            return Ok(
                new
                {
                    message =
                        "Visitor deleted successfully"
                });
        }
    }
}