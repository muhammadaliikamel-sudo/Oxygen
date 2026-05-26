namespace Oxygen.Models;

public class Candidate
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public string Discipline { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string PreferredLocation { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public long ResumeDocumentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid VisitorId { get; set; }
    public bool IsDeleted { get; set; }
}
