namespace Oxygen.Models
{
    public class EntityDocument
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public required string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
