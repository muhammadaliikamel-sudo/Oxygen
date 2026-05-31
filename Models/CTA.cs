namespace Oxygen.Models
{
    public class CTA : BaseEntity
    {
        public Guid InteractionId { get; set; }
        public Guid OwnerId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
        public string? SLAState { get; set; }

    }
}