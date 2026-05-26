namespace Oxygen.Models
{
    public class JobRole
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string RequiredSkills { get; set; }
        public int MinExperience { get; set; }
        public required string Location { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
