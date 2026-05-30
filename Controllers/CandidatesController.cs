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
    public async Task<ActionResult<CandidateResponseDto>> CreateCandidate(
        [FromBody] CreateCandidateDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _candidateService.CreateCandidateAsync(dto, cancellationToken);
            return Created($"/candidates/{response.Id}", response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("candidate-status")]
    public async Task<ActionResult<CandidateResponseDto>> UpdateStatus(
        [FromBody] UpdateCandidateStatusDto dto,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dto.Status))
        {
            return BadRequest("status is required");
        }

        CandidateResponseDto? response;
        try
        {
            response = await _candidateService.UpdateStatusAsync(dto, cancellationToken);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        if (response is null)
        {
            return NotFound("candidate not found");
        }

        return Ok(response);
    }

    [HttpGet("shortlist")]
    public async Task<ActionResult<IReadOnlyList<CandidateShortlistDto>>> GetShortlist(
        [FromQuery] decimal? minScore,
        [FromQuery] int? limit,
        CancellationToken cancellationToken)
    {
        var response = await _candidateService.GetShortlistAsync(
            minScore ?? 70m,
            limit ?? 50,
            cancellationToken);
        return Ok(response);
    }
}
