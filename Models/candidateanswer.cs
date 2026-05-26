namespace Oxygen.Models
{
    public class CandidateAnswer
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }
        public required string Question { get; set; }
        public required string Answer { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
