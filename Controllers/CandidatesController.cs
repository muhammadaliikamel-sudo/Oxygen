using Microsoft.AspNetCore.Mvc;
using Oxygen.DTOs;
using Oxygen.Interfaces;

namespace Oxygen.Controllers;

[ApiController]
public class CandidatesController : ControllerBase
{
    private readonly ICandidateService _candidateService;

    public CandidatesController(ICandidateService candidateService)
    {
        _candidateService = candidateService;
    }

    [HttpPost("candidates")]
    public ActionResult<CandidateResponseDto> CreateCandidate([FromBody] CreateCandidateDto dto)
    {
        var response = _candidateService.CreateCandidate(dto);
        return Created($"/candidates/{response.Id}", response);
    }

    [HttpPatch("candidate-status")]
    public ActionResult<CandidateResponseDto> UpdateStatus([FromBody] UpdateCandidateStatusDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Status))
        {
            return BadRequest("status is required");
        }

        var response = _candidateService.UpdateStatus(dto);
        if (response is null)
        {
            return NotFound("candidate not found");
        }

        return Ok(response);
    }
}
