namespace Oxygen.DTOs.CTA
{
    public class CreateCTADto
    {
        public Guid InteractionId { get; set; }

        public Guid OwnerId { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public DateTime? DueDate { get; set; }
    }
}