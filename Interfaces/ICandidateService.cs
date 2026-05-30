using Oxygen.DTOs;

namespace Oxygen.Interfaces;

public interface ICandidateService
{
    Task<CandidateResponseDto> CreateCandidateAsync(CreateCandidateDto dto, CancellationToken cancellationToken);
    Task<CandidateResponseDto?> UpdateStatusAsync(UpdateCandidateStatusDto dto, CancellationToken cancellationToken);
    Task<IReadOnlyList<CandidateShortlistDto>> GetShortlistAsync(decimal minScore, int limit, CancellationToken cancellationToken);
}
