using Microsoft.EntityFrameworkCore;
using Oxygen.Data;
using Oxygen.DTOs;
using Oxygen.Interfaces;
using Oxygen.Models;

namespace Oxygen.Services;

public class CandidateService : ICandidateService
{
    private readonly AppDbContext _dbContext;

    public CandidateService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CandidateResponseDto> CreateCandidateAsync(
        CreateCandidateDto dto,
        CancellationToken cancellationToken)
    {
        ValidateRequired(dto.FullName, "fullName");
        ValidateRequired(dto.Email, "email");
        ValidateRequired(dto.Phone, "phone");
        ValidateRequired(dto.Discipline, "discipline");

        var allowedStatuses = await GetAllowedStatusesAsync(cancellationToken);
        var status = ResolveStatus(dto.Status, allowedStatuses);
        var now = DateTime.UtcNow;

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
            Status = status,
            ResumeDocumentId = dto.ResumeDocumentId,
            VisitorId = dto.VisitorId,
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false
        };

        _dbContext.Candidates.Add(candidate);

        foreach (var answer in dto.Answers)
        {
            if (string.IsNullOrWhiteSpace(answer.Question) || string.IsNullOrWhiteSpace(answer.Answer))
            {
                continue;
            }

            _dbContext.CandidateAnswers.Add(new CandidateAnswer
            {
                Id = Guid.NewGuid(),
                CandidateId = candidate.Id,
                Question = answer.Question.Trim(),
                Answer = answer.Answer.Trim(),
                CreatedAt = now
            });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        await CreateMatchesAsync(candidate, cancellationToken);

        return MapToResponse(candidate);
    }

    public async Task<CandidateResponseDto?> UpdateStatusAsync(
        UpdateCandidateStatusDto dto,
        CancellationToken cancellationToken)
    {
        var candidate = await _dbContext.Candidates
            .FirstOrDefaultAsync(item => item.Id == dto.CandidateId, cancellationToken);
        if (candidate is null)
        {
            return null;
        }

        var allowedStatuses = await GetAllowedStatusesAsync(cancellationToken);
        var normalizedStatus = dto.Status.Trim();
        if (allowedStatuses.Count > 0 && !allowedStatuses.Contains(normalizedStatus, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException("invalid status");
        }

        candidate.Status = normalizedStatus;
        candidate.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(candidate);
    }

    public async Task<IReadOnlyList<CandidateShortlistDto>> GetShortlistAsync(
        decimal minScore,
        int limit,
        CancellationToken cancellationToken)
    {
        var shortlist = await (from match in _dbContext.CandidateMatches
            join candidate in _dbContext.Candidates on match.CandidateId equals candidate.Id
            join role in _dbContext.JobRoles on match.JobRoleId equals role.Id
            where match.Score >= minScore
            orderby match.Score descending
            select new CandidateShortlistDto
            {
                CandidateId = candidate.Id,
                FullName = candidate.FullName,
                Status = candidate.Status,
                JobRoleId = role.Id,
                JobRoleTitle = role.Title,
                Score = match.Score
            })
            .Take(limit)
            .ToListAsync(cancellationToken);

        return shortlist;
    }

    private async Task CreateMatchesAsync(Candidate candidate, CancellationToken cancellationToken)
    {
        var roles = await _dbContext.JobRoles.ToListAsync(cancellationToken);
        if (roles.Count == 0)
        {
            return;
        }

        var candidateSkills = SplitSkills(candidate.Skills);
        var matches = new List<CandidateMatch>();

        foreach (var role in roles)
        {
            var requiredSkills = SplitSkills(role.RequiredSkills);
            var score = CalculateScore(candidate, candidateSkills, role, requiredSkills);
            matches.Add(new CandidateMatch
            {
                Id = Guid.NewGuid(),
                CandidateId = candidate.Id,
                JobRoleId = role.Id,
                Score = score,
                CreatedAt = DateTime.UtcNow
            });
        }

        _dbContext.CandidateMatches.AddRange(matches);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static decimal CalculateScore(
        Candidate candidate,
        HashSet<string> candidateSkills,
        JobRole role,
        HashSet<string> requiredSkills)
    {
        var matchedSkills = requiredSkills.Count == 0
            ? 0
            : requiredSkills.Count(skill => candidateSkills.Contains(skill));
        var skillRatio = requiredSkills.Count == 0 ? 0m : (decimal)matchedSkills / requiredSkills.Count;

        var experienceScore = role.MinExperience <= 0
            ? 1m
            : Math.Min(1m, (decimal)candidate.ExperienceYears / role.MinExperience);

        var locationScore = string.IsNullOrWhiteSpace(candidate.PreferredLocation)
            ? 0m
            : string.Equals(candidate.PreferredLocation.Trim(), role.Location.Trim(), StringComparison.OrdinalIgnoreCase)
                ? 1m
                : 0m;

        return Math.Round((skillRatio * 70m) + (experienceScore * 20m) + (locationScore * 10m), 2);
    }

    private static HashSet<string> SplitSkills(string skills)
    {
        return skills
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(skill => skill.ToLowerInvariant())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private async Task<IReadOnlyList<string>> GetAllowedStatusesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.LookupLists
            .Where(item => item.Type == "candidate_status")
            .Select(item => item.Value)
            .ToListAsync(cancellationToken);
    }

    private static string ResolveStatus(string? status, IReadOnlyList<string> allowedStatuses)
    {
        var resolved = string.IsNullOrWhiteSpace(status) ? "new" : status.Trim();
        if (allowedStatuses.Count == 0)
        {
            return resolved;
        }

        if (allowedStatuses.Contains(resolved, StringComparer.OrdinalIgnoreCase))
        {
            return resolved;
        }

        return allowedStatuses[0];
    }

    private static void ValidateRequired(string value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{field} is required");
        }
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
            Status = candidate.Status,
            ResumeDocumentId = candidate.ResumeDocumentId,
            VisitorId = candidate.VisitorId
        };
    }
}
