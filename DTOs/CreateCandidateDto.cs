namespace Oxygen.DTOs;

public class CreateCandidateDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public string Discipline { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string PreferredLocation { get; set; } = string.Empty;
    public string? Status { get; set; }
    public Guid? ResumeDocumentId { get; set; }
    public Guid? VisitorId { get; set; }
    public List<CandidateAnswerDto> Answers { get; set; } = new();
}
