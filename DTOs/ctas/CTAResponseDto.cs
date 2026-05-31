namespace Oxygen.DTOs.CTA
{
    public class CTAResponseDto
    {
        public Guid Id { get; set; }
        public Guid? InteractionId { get; set; }
        public Guid? OwnerId { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? SLAState { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}