using Oxygen.DTOs;

namespace Oxygen.Interfaces;

public interface ICandidateService
{
    CandidateResponseDto CreateCandidate(CreateCandidateDto dto);
    CandidateResponseDto? UpdateStatus(UpdateCandidateStatusDto dto);
}
