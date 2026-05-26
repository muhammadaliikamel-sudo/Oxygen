namespace Oxygen.DTOs;

public class UpdateCandidateStatusDto
{
    public Guid CandidateId { get; set; }
    public string Status { get; set; } = string.Empty;
}
