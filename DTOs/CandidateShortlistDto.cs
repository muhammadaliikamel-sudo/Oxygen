namespace Oxygen.DTOs;

public class CandidateShortlistDto
{
    public Guid CandidateId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Guid JobRoleId { get; set; }
    public string JobRoleTitle { get; set; } = string.Empty;
    public decimal Score { get; set; }
}
