namespace Oxygen.Models
{
    public class CTA : BaseEntity
    {
        public Guid InteractionId { get; set; }

        public required string Title { get; set; }


        public required string Description { get; set; }

        public required string Status { get; set; }

        public required string Priority { get; set; }

        public DateTime DueDate { get; set; }
        public required string Notes { get; set; }

        public required string SLAStatus { get; set; }

        public required string Interaction { get; set; }

    }
}