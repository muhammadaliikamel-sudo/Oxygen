namespace Oxygen.Models
{
    public class Interaction : BaseEntity
    {
        public Guid VisitorId { get; set; }

        public required string Type { get; set; }

        public required string Notes { get; set; }

        public required string Duration { get; set; }

        public required string Visitor { get; set; }

    }
}