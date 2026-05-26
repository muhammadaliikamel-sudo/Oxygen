using System.Collections.Concurrent;
using Oxygen.DTOs;
using Oxygen.Interfaces;
using Oxygen.Models;

namespace Oxygen.Services;

public class CandidateService : ICandidateService
{
    private readonly ConcurrentDictionary<Guid, Candidate> _candidates = new();

    public CandidateResponseDto CreateCandidate(CreateCandidateDto dto)
    {
        var status = string.IsNullOrWhiteSpace(dto.Status) ? "new" : dto.Status.Trim();
        var candidate = new Candidate
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim(),
            Phone = dto.Phone.Trim(),
            ExperienceYears = dto.ExperienceYears,
            Discipline = dto.Discipline.Trim(),
            Skills = dto.Skills.Trim(),
            PreferredLocation = dto.PreferredLocation.Trim(),
            Status = status
        };

        _candidates[candidate.Id] = candidate;
        return MapToResponse(candidate);
    }

    public CandidateResponseDto? UpdateStatus(UpdateCandidateStatusDto dto)
    {
        if (!_candidates.TryGetValue(dto.CandidateId, out var candidate))
        {
            return null;
        }

        candidate.Status = dto.Status.Trim();
        return MapToResponse(candidate);
    }

    private static CandidateResponseDto MapToResponse(Candidate candidate)
    {
        return new CandidateResponseDto
        {
            Id = candidate.Id,
            FullName = candidate.FullName,
            Email = candidate.Email,
            Phone = candidate.Phone,
            ExperienceYears = candidate.ExperienceYears,
            Discipline = candidate.Discipline,
            Skills = candidate.Skills,
            PreferredLocation = candidate.PreferredLocation,
            Status = candidate.Status
        };
    }
}
