namespace Oxygen.Models
{
    public class BackgroundJobRun
    {
        public Guid Id { get; set; }
        public required string JobName { get; set; }
        public required string Status { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Details { get; set; }
    }
}
