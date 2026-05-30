namespace Oxygen.DTOs.Interaction
{
    public class InteractionResponseDto
    {
        public Guid Id { get; set; }

        public Guid? VisitorId { get; set; }

        public string? Type { get; set; }

        public string? Notes { get; set; }

        public int? DurationMinutes { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}