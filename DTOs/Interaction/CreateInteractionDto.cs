namespace Oxygen.DTOs.Interaction
{
    public class CreateInteractionDto
    {
        public Guid VisitorId { get; set; }
        public string? Type { get; set; }
        public string? Notes { get; set; }
        public int? DurationMinutes { get; set; }
    }
}
